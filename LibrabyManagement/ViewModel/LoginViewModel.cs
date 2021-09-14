
using LibrabyManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibrabyManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        // Tài khoản
        private string _username;
        public string username { get => _username; set { _username = value; OnPropertyChanged(); } }

        // Mật Khẩu
        private string _password;
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
                                                                 Login(p);
                                                             });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; },
              (p) =>
              {
                  password = p.Password;
              });
        }

        private void Login(Window window)
        {
            //if (window == null)
            //    return;

            //var User = DataProvider.Ins.DB.Users.Where(x => x.Name == username && x.Password == password);

            //if (User.Count() > 0)
            //{
            //    MessageBox.Show("Đăng nhập thành công");
            //}
            //else
            //{
            //    MessageBox.Show(password, "Đăng nhập thất bại", MessageBoxButton.OK);
            //}
        }
    }
}
