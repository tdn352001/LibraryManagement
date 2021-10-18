using LibraryManagement.Model;
using LibraryManagement.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace LibraryManagement.ViewModel
{
    public class FeeViewModel : BaseViewModel
    {

        // Danh sách các nhà sách trong database
        private ObservableCollection<Fee> _FeeList { get; set; }
        public ObservableCollection<Fee> FeeList { get => _FeeList; set { _FeeList = value; OnPropertyChanged(); } }

        // nhà sách được chọn trên list view
        private Fee _SelectedItem;
        public Fee SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;

                OnPropertyChanged();
            }
        }

        // nhà sách được thêm
        private Fee _AddItem;
        public Fee AddItem
        {
            get => _AddItem;
            set
            {
                _AddItem = value;
                OnPropertyChanged();
            }
        }

        // kiểm tra thông tin đang hiển thị là của item được chọn hay của item muốn thêm
        private bool _AddLayoutVisible = false;
        public bool AddLayoutVisible { get => _AddLayoutVisible; set { _AddLayoutVisible = value; OnPropertyChanged(); } }

        // thứ tự của heading được chọn trong phân loại tìm kiếm trong commbo box
        private int _SearchHeading = 1;
        public int SearchHeading { get => _SearchHeading; set { _SearchHeading = value; OnPropertyChanged(); } }

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
        // Command xử lí khi ấn vào nút tìm kiếm 
        public ICommand SearchCommand { get; set; }
        // Command xử lí khi ấn vào nút lịch sử nhập sách
        public ICommand ExportInfoCommand { get; set; }
        //Command xử lí khi key word thay đổi
        public ICommand KeyWordChangeCommand { get; set; }

        public FeeViewModel()
        {
            // lấy danh sách từ database
            FeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees);

            EditCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  if (FeeValidate(SelectedItem))
                                                                      SaveChangeFee();
                                                              });

            AddCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  AddLayoutVisible = true;
                                                                  AddItem = new Fee();
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
                                                                 if (FeeValidate(AddItem))
                                                                     SaveAddFee();
                                                             });

            KeyWordChangeCommand = new RelayCommand<TextBox>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 DisplayResultSearch(p.Text);
                                                             });

            DeleteCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 DeleteFee();
                                                             });
        }



        private void SaveChangeFee()
        {
            if (SelectedItem != null)
            {
                var fee = DataProvider.Ins.DB.Fees.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                fee.Interval = SelectedItem.Interval;
                fee.Money = SelectedItem.Money;
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Đã Lưu", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveAddFee()
        {
            if (AddItem != null)
            {
                DataProvider.Ins.DB.Fees.Add(AddItem);
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem = AddItem;
                FeeList.Add(AddItem);
                AddItem = null;
                AddLayoutVisible = false;
            }
        }

        private Boolean FeeValidate(Fee fee)
        {
            if (fee == null)
                return false;

            return true;
        }

        private void DisplayResultSearch(string keyWord)
        {
            FeeList.Clear();
            if (String.IsNullOrWhiteSpace(keyWord))
            {
                FeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees);
                return;
            }
            else
            {
                switch (SearchHeading)
                {
                    case 0:
                        FeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees.Where(x => x.Id.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    //case 1:
                    //    FeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees.Where(x => x.Name.ToString().ToLower().Contains(keyWord.ToLower())));
                    //    break;
                    //case 2:
                    //    StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Address.ToString().ToLower().Contains(keyWord.ToLower())));
                    //    break;
                    //case 3:
                    //    StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Phone.ToString().ToLower().Contains(keyWord.ToLower())));
                    //    break;
                }
            }
        }


        private void DeleteFee()
        {
            if (SelectedItem == null)
                return;

            String title = "Xóa loại phí?";
            String message = "Hành động này không thể khôi phục, Bạn có chắc chắn?";
            MessageBoxResult dialogResult = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
               
                DataProvider.Ins.DB.Fees.Remove(SelectedItem);
                DataProvider.Ins.DB.SaveChanges();
                FeeList.Remove(SelectedItem);
                String notifyTitle = "Thông báo";
                String notifyMessage = "Xóa thành công.";
                MessageBox.Show(notifyMessage, notifyTitle, MessageBoxButton.OK);
            }
        }
    }
}
