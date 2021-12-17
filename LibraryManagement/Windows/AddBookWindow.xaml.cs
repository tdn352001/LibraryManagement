using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LibraryManagement.Windows {
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window {
        public Book book;
        public AddBookWindow() {
            InitializeComponent();
            book = new Book();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e) {
            if(tbNameBook.Text.Length < 4) {
                MessageBox.Show("Tên sách có độ dài tối thiểu là 4", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            book.Name = tbNameBook.Text;
            book.Author = tbAuthor.Text;
            book.MoreInfo = tbMoreInfo.Text;
            book.Quantity = 0;
            DataProvider.Ins.DB.Books.Add(book);
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Thêm thành công, mã sách " + book.Name + " là: " + book.Id, "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
