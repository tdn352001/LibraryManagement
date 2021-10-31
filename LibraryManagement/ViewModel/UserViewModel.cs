using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Windows.Controls;


namespace LibraryManagement.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        // Danh sách các độc giả trong database
        private ObservableCollection<User> _UserList;
        public ObservableCollection<User> UserList { get => _UserList; set { _UserList = value; OnPropertyChanged(); } }

        private ObservableCollection<UserStatu> _UserStatus;
        public ObservableCollection<UserStatu> UserStatus { get => _UserStatus; set { _UserStatus = value; OnPropertyChanged(); } }

        private User _SelectedItem;
        public User SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;

                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Name = SelectedItem.Name;
                    BirthDay = SelectedItem.BirthDay;
                    Email = SelectedItem.Email;
                    Address = SelectedItem.Address;
                    Phone = SelectedItem.Phone;
                    SelectedUserStatus = SelectedItem.UserStatu;
                }
                    
            }
        }

        private UserStatu _SelectedUserStatus;
        public UserStatu SelectedUserStatus
        {
            get => _SelectedUserStatus;
            set
            {
                _SelectedUserStatus = value;

                OnPropertyChanged();
            }
        }

        private User _AddItem;
        public User AddItem
        {
            get => _AddItem;
            set
            {
                _AddItem = value;
                OnPropertyChanged();
                if (AddItem != null)
                {
                    SelectedUserStatus = AddItem.UserStatu;
                }
            }
        }

        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private DateTime? _BirthDay;
        public DateTime? BirthDay { get => _BirthDay; set { _BirthDay = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }










        // kiểm tra thông tin đang hiển thị là của item được chọn hay của item muốn thêm
        private bool _AddLayoutVisible = false;
        public bool AddLayoutVisible { get => _AddLayoutVisible; set { _AddLayoutVisible = value; OnPropertyChanged(); } }

        //Command xử lí khi click vào nút thêm (nút trên list view)
        public ICommand AddCommand { get; set; }

        // Command xử lí khi ấn vào nút lưu trong layout thêm item
        public ICommand SaveAddCommand { get; set; }

        // Command xử lí khi ấn vào nút hủy trong layout thêm item
        public ICommand CancelAddCommand { get; set; }

        // Command xử lí khi ấn vào nút lưu trong layout selected item
        public ICommand EditCommand { get; set; }

        // Command xử lí khi ấn vào nút xóa trong layout selected item
        public ICommand DeleteCommand { get; set; }

        // Command xử lí khi ấn vào nút xuất thông tin
        public ICommand ExportInfoCommand { get; set; }









        // thứ tự của heading được chọn trong phân loại tìm kiếm trong commbo box
        private int _SearchHeading = 1;
        public int SearchHeading { get => _SearchHeading; set { _SearchHeading = value; OnPropertyChanged(); } }

        //Command xử lí khi key word thay đổi
        public ICommand KeyWordChangeCommand { get; set; }

        public UserViewModel()
        {
           
            // lấy danh sách độc giả từ database
            UserList = new ObservableCollection<User>(DataProvider.Ins.DB.Users.ToList());
            UserStatus = new ObservableCollection<UserStatu>(DataProvider.Ins.DB.UserStatus);

            EditCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 if (UserValidate(SelectedItem))
                                                                     SaveChangeUser();
                                                             });

            AddCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  AddLayoutVisible = true;
                                                                  AddItem = new User();
                                                                  DateTime now = DateTime.Now;
                                                                  AddItem.BirthDay = now;
                                                                  OnPropertyChanged("AddItem");

                                                              });


            CancelAddCommand = new RelayCommand<Object>((p) => { return true; },
                                                               (p) =>
                                                               {
                                                                   AddLayoutVisible = false;
                                                                   AddItem = null;

                                                               });

            SaveAddCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 if (UserValidate(AddItem))
                                                                     SaveAddUser();
                                                             });

            DeleteCommand = new RelayCommand<System.Windows.Controls.TextBox>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 DeleteUser();
                                                             });

            ExportInfoCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 ExportUser();
                                                             });




            KeyWordChangeCommand = new RelayCommand<System.Windows.Controls.TextBox>((p) => { return true; },
                                                            (p) =>
                                                            {
                                                                DisplayResultSearch(p.Text);
                                                            });
        }
        

        private void DisplayResultSearch(string keyWord)
        {
            UserList.Clear();
            if (String.IsNullOrWhiteSpace(keyWord))
            {
                UserList = new ObservableCollection<User>(DataProvider.Ins.DB.Users);
                return;
            }
            else
            {
                switch (SearchHeading)
                {
                    case 0:
                        UserList = new ObservableCollection<User>(DataProvider.Ins.DB.Users.Where(x => x.Id.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 1:
                        UserList = new ObservableCollection<User>(DataProvider.Ins.DB.Users.Where(x => x.Name.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                }
            }
        }

        private void SaveChangeUser()
        {
            if (SelectedItem != null)
            {
                var user = DataProvider.Ins.DB.Users.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                ////SelectedItem.IdStatus = SelectedUserStatus.Id;
                
                user.Name = Name;
                user.BirthDay = BirthDay;
                user.Address = Address;
                user.Email = Email;
                user.Phone = Phone;
                user.IdStatus = SelectedUserStatus.Id;
                DataProvider.Ins.DB.SaveChanges();

                SelectedItem.Name = Name;
                MessageBox.Show("Đã Lưu", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

            private void SaveAddUser()
        {
            if (AddItem != null)
            {
                DataProvider.Ins.DB.Users.Add(AddItem);
                AddItem.IdStatus = SelectedUserStatus.Id;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem = AddItem;
                UserList.Add(AddItem);
                AddItem = null;
                AddLayoutVisible = false;
            }
        }


        



        private Boolean UserValidate(User user)
        {
            if (user == null)
                return false;

            if (String.IsNullOrEmpty(user.Name))
                return false;

            if (String.IsNullOrEmpty(user.Address))
                return false;

            return true;
        }

        private void DeleteUser()
        {
            if (SelectedItem == null) return;

            String title = "Xóa thành viên ?";
            String message = "Hành động này không thể khôi phục, Bạn có chắc chắn?";
            MessageBoxResult dialogResult = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (SelectedItem.DetailFees.Count > 0)
                {
                    String notifyTitle = "Thông báo";
                    String notifyMessage = "Không thể xóa thành viên này.";
                    MessageBox.Show(notifyMessage, notifyTitle, MessageBoxButton.OK);
                }
                else
                {
                    
                    DataProvider.Ins.DB.Users.Remove(SelectedItem);
                    SelectedItem.IdStatus = SelectedUserStatus.Id;
                    DataProvider.Ins.DB.SaveChanges();
                    UserList.Remove(SelectedItem);
                    String notifyTitle = "Thông báo";
                    String notifyMessage = "Xóa thành công.";
                    MessageBox.Show(notifyMessage, notifyTitle, MessageBoxButton.OK);
                }
            }
                

        }

        private void ExportUser()
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook |*.xls", ValidateNames = true };
            if (sfd.ShowDialog() == true)
            {

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                Worksheet ws = (Worksheet)app.ActiveSheet;
                app.Visible = false;
                ws.Cells[1, 1] = "Mã độc giả";
                ws.Cells[1, 2] = "Tên độc giả";
                ws.Cells[1, 3] = "Ngày sinh";
                ws.Cells[1, 4] = "Email";
                ws.Cells[1, 5] = "Địa chỉ";
                ws.Cells[1, 6] = "SĐT";
                ws.Cells[1, 7] = "Trạng thái";
                int i = 2;
                foreach (var item in UserList)
                {
                    ws.Cells[i, 1] = item.Id;
                    ws.Cells[i, 2] = item.Name;
                    ws.Cells[i, 3] = item.BirthDay;
                    ws.Cells[i, 4] = item.Email;
                    ws.Cells[i, 5] = item.Address;
                    ws.Cells[i, 6] = item.Phone;
                    ws.Cells[i, 7] = item.UserStatu.Name;
                    i++;
                }
                wb.SaveAs(sfd.FileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                app.Quit();
                MessageBox.Show("Your data has been successfully exported. ");


            }


        }





    }
}
