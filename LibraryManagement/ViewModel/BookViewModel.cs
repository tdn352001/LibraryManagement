using LibraryManagement.Model;
using LibraryManagement.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;

namespace LibraryManagement.ViewModel
{
    public class BookViewModel : BaseViewModel
    {

        // Danh sách sách trong db
        private ObservableCollection<Book> _BookList { get; set; }
        public ObservableCollection<Book> BookList { get => _BookList; set { _BookList = value; OnPropertyChanged(); } }

        // sách được chọn trên list view
        private Book _SelectedItem;
        public Book SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;

                OnPropertyChanged();
            }
        }

        // sách được thêm
        private Book _AddItem;
        public Book AddItem
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
        
        //Command xử lí khi key word thay đổi
        public ICommand KeyWordChangeCommand { get; set; }

        //Command xử lí xuất thông tin
        public ICommand ExportInfoCommand { get; set; }


        public BookViewModel()
        {
            // lấy danh sách từ database
            BookList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books);

            EditCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  if (BookValidate(SelectedItem))
                                                                      SaveChangeBook();
                                                              });

            AddCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  AddLayoutVisible = true;
                                                                  AddItem = new Book();
                                                                  AddItem.Quantity = 1;
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
                                                                 if (BookValidate(AddItem))
                                                                     SaveAddBook();
                                                             });

            KeyWordChangeCommand = new RelayCommand<System.Windows.Controls.TextBox>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 DisplayResultSearch(p.Text);
                                                             });

            DeleteCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 DeleteBook();
                                                             });

            ExportInfoCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) =>
                                                             {
                                                                 ExportBook();
                                                             });
        }



        private void SaveChangeBook()
        {
            if (SelectedItem != null)
            {
                var store = DataProvider.Ins.DB.Books.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                store.Name = SelectedItem.Name;
                store.Author = SelectedItem.Author;
                store.MoreInfo = SelectedItem.MoreInfo;
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Đã Lưu", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveAddBook()
        {
            if (AddItem != null)
            {
                DataProvider.Ins.DB.Books.Add(AddItem);
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem = AddItem;
                BookList.Add(AddItem);
                AddItem = null;
                AddLayoutVisible = false;
            }
        }

        private Boolean BookValidate(Book book)
        {
            if (book == null)
                return false;

            if (String.IsNullOrEmpty(book.Name))
                return false;

            if (String.IsNullOrEmpty(book.Author))
                return false;

            return true;
        }

        private void DisplayResultSearch(string keyWord)
        {
            BookList.Clear();
            if (String.IsNullOrWhiteSpace(keyWord))
            {
                BookList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books);
                return;
            }
            else
            {
                switch (SearchHeading)
                {
                    case 0:
                        BookList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books.Where(x => x.Id.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 1:
                        BookList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books.Where(x => x.Name.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 2:
                        BookList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books.Where(x => x.Author.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;

                }
            }
        }

        
        private void DeleteBook()
        {
            if (SelectedItem == null)
                return;

            String title = "Xóa sách?";
            String message = "Bạn muốn xóa sách đang chọn ?";
            MessageBoxResult dialogResult = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                
                DataProvider.Ins.DB.Books.Remove(SelectedItem);
                DataProvider.Ins.DB.SaveChanges();
                BookList.Remove(SelectedItem);
                String notifyTitle = "Thông báo";
                String notifyMessage = "Xóa thành công.";
                MessageBox.Show(notifyMessage, notifyTitle, MessageBoxButton.OK);
                
            }
        }
        public void ExportBook()
        {
            SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook |*.xls", ValidateNames = true };
            if (sfd.ShowDialog() == true)
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                Worksheet ws = (Worksheet)app.ActiveSheet;

                app.Visible = false;
                ws.Cells[1, 1] = "Mã sách";
                ws.Cells[1, 2] = "Tên sách";
                ws.Cells[1, 3] = "Tác giả";
                ws.Cells[1, 4] = "Ngày xuất bản";
                ws.Cells[1, 5] = "Giá mua";
                ws.Cells[1, 6] = "Giá bán";
                ws.Cells[1, 7] = "Trạng thái";
                ws.Cells[1, 8] = "Thông tin thêm";

                int i = 2;
                foreach (var item in BookList)
                {
                    ws.Cells[i, 1] = item.Id;
                    ws.Cells[i, 2] = item.Name;
                    ws.Cells[i, 3] = item.Author;
                    ws.Cells[i, 4] = item.PublishDate;
                    ws.Cells[i, 5] = item.PriceIn;
                    ws.Cells[i, 6] = item.PriceOut;
                    ws.Cells[i, 7] = item.Quantity;
                    ws.Cells[i, 8] = item.MoreInfo;
                }
                wb.SaveAs(sfd.FileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                app.Quit();

                MessageBox.Show("Your data has been successfully exported. ");
            }
        }
    }
}
