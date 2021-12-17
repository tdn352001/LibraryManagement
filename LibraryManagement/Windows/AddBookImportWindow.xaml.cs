using LibraryManagement.Model;
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

namespace LibraryManagement.Windows {
    /// <summary>
    /// Interaction logic for AddBookImportWindow.xaml
    /// </summary>
    public partial class AddBookImportWindow : Window {

        public Book book;
        public DetailImport detailImport;
        public Boolean isAdd = false;

        public AddBookImportWindow() {
            InitializeComponent();
            detailImport = new DetailImport();
        }

        private void number_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbIdBook_TextChanged(object sender, TextChangedEventArgs e) {
            String idBook = tbIdBook.Text;
            try {
                int id = int.Parse(idBook);
                book = DataProvider.Ins.DB.Books.Where(x => x.Id == id).SingleOrDefault();
                if(book != null) {
                    tbNameBook.Text = book.Name;
                }
                else {
                    tbNameBook.Text = "";
                }
            }
            catch (Exception) {
                tbNameBook.Text = "";
                book = null;
            }
        }

        private void tbQuantity_TextChanged(object sender, TextChangedEventArgs e) {
            String strQuantity = tbQuantity.Text;
            String strPrice = tbPriceIn.Text;
            try {
                int quantity = int.Parse(strQuantity);
                detailImport.Quantity = quantity;
                try {
                    int price = int.Parse(strPrice);
                    int totalPrice = quantity * price;
                    tbTotalPrice.Text = totalPrice.ToString();
                }catch (Exception) { }
            }
            catch (Exception) {
                detailImport.Quantity = -1;
                tbTotalPrice.Text = "0";
            }
        }

        private void tbPriceIn_TextChanged(object sender, TextChangedEventArgs e) {
            String strPrice = tbPriceIn.Text;
            String strQuantity = tbQuantity.Text;
            try {
                int price = int.Parse(strPrice);
                detailImport.PriceIn = price;
                try {
                    int quantity = int.Parse(strQuantity);
                    int totalPrice = quantity * price;
                    tbTotalPrice.Text = totalPrice.ToString();
                }
                catch (Exception) { }
            }
            catch (Exception) {
                detailImport.PriceIn = -1;
                tbTotalPrice.Text = "0";
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e) {
            if (isValidate()) {
                AddDetail();
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private Boolean isValidate() {
            if(book == null) {
                MessageBox.Show("Không tìm thấy sách", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(detailImport.Quantity == null || detailImport.Quantity == -1) {
                MessageBox.Show("Vui lòng nhập số lượng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (detailImport.PriceIn == null || detailImport.PriceIn == -1) {
                MessageBox.Show("Vui lòng nhập giá tiền", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void AddDetail() {
            detailImport.Book = book;
            detailImport.IdBook = book.Id;
            this.isAdd = true;
            this.Close();
        }

        private void addBookBtn_Click(object sender, RoutedEventArgs e) {
            AddBookWindow window = new AddBookWindow();
            window.ShowDialog();
        }
    }
}
