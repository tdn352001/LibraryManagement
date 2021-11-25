using LibraryManagement.Model;
using LibraryManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryManagement.Windows
{
    /// <summary>
    /// Interaction logic for BorrowBookWindow.xaml
    /// </summary>
    public partial class BorrowBookWindow : Window
    {
        public User user;
        public Book book1;
        public Book book2;
        public ICommand SaveCommand;
        public BorrowBookWindow()
        {
            InitializeComponent();
            SetCurrentDate();
        }

        public BorrowBookWindow(User user)
        {
            InitializeComponent();
            SetCurrentDate();
            this.user = user;
            if (user != null)
                tbIdUser.Text = user.Name;
        }

        public void SetCurrentDate()
        {
            dpTime.SelectedDate = DateTime.Now;
        }


        private void tbIdUser_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void tbIdUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbIdUser.Text.Equals(""))
            {
                tbNameUser.Text = "";
                user = null;
                return;
            }
            int id = int.Parse(tbIdUser.Text);
            user = DataProvider.Ins.DB.Users.Where(x => x.Id == id).SingleOrDefault();
            tbNameUser.Text = user != null ? user.Name : "";
        }

        private void addBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Book2Container.Visibility == Visibility.Visible)
            {
                Book2Container.Visibility = Visibility.Collapsed;
                addBookBtnContent.Text = "Thêm sách";
                var data = Application.Current.TryFindResource("Add");
                addBookBtnIcon.Data = (Geometry)data;
                book2 = null;
            }
            else
            {
                Book2Container.Visibility = Visibility.Visible;
                addBookBtnContent.Text = "Bỏ sách";
                var data = Application.Current.TryFindResource("Cancel");
                addBookBtnIcon.Data = (Geometry)data;
            }
        }

        private void tbIdBook1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbIdBook1.Text.Equals(""))
            {
                tbNameBook1.Text = "";
                book1 = null;
                return;
            }
            int id = int.Parse(tbIdBook1.Text);
            book1 = DataProvider.Ins.DB.Books.Where(x => x.Id == id).SingleOrDefault();
            tbNameBook1.Text = book1 != null ? book1.Name : "";
        }

        private void tbIdBook2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbIdBook2.Text.Equals(""))
            {
                tbNameBook2.Text = "";
                book2 = null;
                return;
            }
            int id = int.Parse(tbIdBook2.Text);
            book2 = DataProvider.Ins.DB.Books.Where(x => x.Id == id).SingleOrDefault();
            tbNameBook2.Text = book2 != null ? book2.Name : "";
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateBook())
            {
                HistoryBook historyBook1 = new HistoryBook();
                historyBook1.BorrowedDate = dpTime.SelectedDate;
                historyBook1.Book = book1;
                historyBook1.User = user;
                historyBook1.IdStatus = 1;
                DataProvider.Ins.DB.HistoryBooks.Add(historyBook1);

                if(Book2Container.Visibility == Visibility.Visible && book2 != null)
                {
                    HistoryBook historyBook2 = new HistoryBook();
                    historyBook2.BorrowedDate = dpTime.SelectedDate;
                    historyBook2.Book = book2;
                    historyBook2.User = user;
                    historyBook2.IdStatus = 1;
                    DataProvider.Ins.DB.HistoryBooks.Add(historyBook2);
                }
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Lưu thành công", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }



        private bool ValidateBook()
        {
            if (user == null)
            {
                MessageBox.Show("Không tìm thấy người dùng", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (book1 == null)
            {
                MessageBox.Show("Vui lòng nhập đúng thông tin sách", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (Book2Container.Visibility == Visibility.Visible)
            {
                if (book2 == null)
                {
                    MessageBox.Show("Vui lòng nhập đúng thông tin sách", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (book2.Id.Equals(book1.Id))
                {
                    MessageBox.Show("Hai sách không được trùng nhau", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
    }
}
