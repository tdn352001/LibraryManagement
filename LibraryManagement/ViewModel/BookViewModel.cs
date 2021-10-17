using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class BookViewModel : BaseViewModel
    {
        // Danh sách các độc giả trong database
        private ObservableCollection<Book> _BookList;
        public ObservableCollection<Book> UserList { get => _BookList; set { _BookList = value; OnPropertyChanged(); } }

        // Danh sách lịch sử mượn sách của các độc giả
        private ObservableCollection<HistoryBook> _HistoryBookList;

        public ObservableCollection<HistoryBook> HistoryBookList { get => _HistoryBookList; set { _HistoryBookList = value; OnPropertyChanged(); } }

        // thứ tự của heading được chọn trong phân loại tìm kiếm trong commbo box
        private int _SearchHeading = 1;
        public int SearchHeading { get => _SearchHeading; set { _SearchHeading = value; OnPropertyChanged(); } }

        //Command xử lí khi key word thay đổi
        public ICommand KeyWordChangeCommand { get; set; }

        public BookViewModel()
        {
            // lấy danh sách độc giả từ database
            UserList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books.ToList());

            // lấy danh sách lịch sử mượn sách của độc giả từ database
            HistoryBookList = new ObservableCollection<HistoryBook>(DataProvider.Ins.DB.HistoryBooks.ToList());

            //
            KeyWordChangeCommand = new RelayCommand<TextBox>((p) => { return true; },
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
                UserList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books);
                return;
            }
            else
            {
                switch (SearchHeading)
                {
                    case 0:
                        UserList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books.Where(x => x.Id.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 1:
                        UserList = new ObservableCollection<Book>(DataProvider.Ins.DB.Books.Where(x => x.Name.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                }
            }
        }

    }
}
