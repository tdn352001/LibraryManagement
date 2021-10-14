using LibraryManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class StoreViewModel : BaseViewModel
    {

        // Danh sách các nhà sách trong database
        private ObservableCollection<BookStore> _StoreList { get; set; }
        public ObservableCollection<BookStore> StoreList { get => _StoreList; set { _StoreList = value; OnPropertyChanged(); } }

        // nhà sách được chọn trên list view
        private BookStore _SelectedItem;
        public BookStore SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;

                OnPropertyChanged();
            }
        }

        // nhà sách được thêm
        private BookStore _AddItem;
        public BookStore AddItem
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
        public ICommand HistoryStoreCommand { get; set; }
        // Command xử lí khi ấn vào nút xuất thông tin
        public ICommand ExportInfoCommand { get; set; }
        //Command xử lí khi key word thay đổi
        public ICommand KeyWordChangeCommand { get; set; }

        public StoreViewModel()
        {
            // lấy danh sách từ database
            StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores);

            EditCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  if (BookStoreValidate(SelectedItem))
                                                                      SaveChangeStore();
                                                              });

            AddCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  AddLayoutVisible = true;
                                                                  AddItem = new BookStore();
                                                                  DateTime now = DateTime.Now;
                                                                  AddItem.CoopDate = now;
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
                                                                 if (BookStoreValidate(AddItem))
                                                                     SaveAddStore();
                                                             });

            KeyWordChangeCommand = new RelayCommand<TextBox>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 DisplayResultSearch(p.Text);
                                                             });

            DeleteCommand = new RelayCommand<TextBox>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 DeleteStore();
                                                             });

        }



        private void SaveChangeStore()
        {
            if (SelectedItem != null)
            {
                var store = DataProvider.Ins.DB.BookStores.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                store.Name = SelectedItem.Name;
                store.Address = SelectedItem.Address;
                store.MoreInfo = SelectedItem.MoreInfo;
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Đã Lưu", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveAddStore()
        {
            if (AddItem != null)
            {
                DataProvider.Ins.DB.BookStores.Add(AddItem);
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem = AddItem;
                StoreList.Add(AddItem);
                AddItem = null;
                AddLayoutVisible = false;
            }
        }

        private Boolean BookStoreValidate(BookStore store)
        {
            if (store == null)
                return false;

            if (String.IsNullOrEmpty(store.Name))
                return false;

            if (String.IsNullOrEmpty(store.Address))
                return false;

            return true;
        }

        private void DisplayResultSearch(string keyWord)
        {
            StoreList.Clear();
            if (String.IsNullOrWhiteSpace(keyWord))
            {
                StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores);
                return;
            }
            else
            {
                switch (SearchHeading)
                {
                    case 0:
                        StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Id.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 1:
                        StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Name.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 2:
                        StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Address.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 3:
                        StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Phone.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                }
            }
        }


        private void DeleteStore()
        {
            if(SelectedItem == null) return;

            String title = "Xóa nhà sách?";
            String message = "Hành động này không thể khôi phục, Bạn có chắc chắn?";
            MessageBoxResult dialogResult = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            if(dialogResult == MessageBoxResult.Yes)
            {
                if (SelectedItem.ImportBooks != null)
                {
                    String notifyTitle = "Thông báo";
                    String notifyMessage = "Không thể xóa nhà sách này.";
                    MessageBox.Show(notifyMessage, notifyTitle, MessageBoxButton.OK);
                }
                else
                {
                    DataProvider.Ins.DB.BookStores.Remove(SelectedItem);
                    DataProvider.Ins.DB.SaveChanges();
                    StoreList.Remove(SelectedItem);
                    String notifyTitle = "Thông báo";
                    String notifyMessage = "Xóa thành công.";
                    MessageBox.Show(notifyMessage, notifyTitle, MessageBoxButton.OK);
                }
            }


        }
    }
}
