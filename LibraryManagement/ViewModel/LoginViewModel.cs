using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        // Tài khoản
        private string _username = "";
        public string username { get => _username; set { _username = value; OnPropertyChanged(); } }

        // Mật Khẩu
        private string _password = "";
        public string password { get => _password; set { _password = value; OnPropertyChanged(); } }


        // Command di chuyển cửa sổ đăng nhập
        public ICommand MoveWindowCommand { get; set; }

        // Commnand đóng của sổ đăng nhập
        public ICommand CloseWindowCommand { get; set; }

        //Command đăng nhập
        public ICommand LoginCommand { get; set; }

        // Commnad lấy mật khẩu :>
        public ICommand PasswordChangedCommand { get; set; }

        // Contructor
        public LoginViewModel()
        {
            // xử lí di chuyển của sổ
            MoveWindowCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; },
                                                              (p) => {
                                                                  p.DragMove();
                                                              });

            // xử lí đóng của sổ
            CloseWindowCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; },
                                                             (p) => {
                                                                 p.Close();
                                                             });

            // Xử lí đăng nhập
            LoginCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; },
                                                             (p) => {
                                                                 Login(password, p);
                                                             });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; },
              (p) =>
              {
                  password = p.Password;
              });
        }

        private string convertPasssword(string input) {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++) {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        private void Login(string password, Window window)
        {

            if(username.Equals("") || password.Equals("")) {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            string convertPassword = convertPasssword(password);
            int countAcount = DataProvider.Ins.DB.Admins.Where(x => x.Name == username && x.Password == convertPassword).Count();
            Admin admin = DataProvider.Ins.DB.Admins.Where(x => x.Username == username && x.Password == convertPassword).SingleOrDefault();

            if (admin == null) {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác", "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                MainWindow mainWindow = new MainWindow();
                window.Hide();
                mainWindow.ShowDialog();
                window.Close();
            }

        }
    }
}
