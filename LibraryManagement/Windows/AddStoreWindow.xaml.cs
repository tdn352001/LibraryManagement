using LibraryManagement.Model;
using LibraryManagement.utils;
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
    /// Interaction logic for AddStoreWindow.xaml
    /// </summary>
    public partial class AddStoreWindow : Window {
        public AddStoreWindow() {
            InitializeComponent();
            tbDate.SelectedDate = DateTime.Now;
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e) {
            BookStore store = new BookStore();
            store.Name = tbName.Text;
            store.Address = tbAddress.Text;
            store.Phone = tbPhone.Text;
            store.CoopDate = tbDate.SelectedDate;
            store.Email = tbEmail.Text;
            store.MoreInfo = tbInfo.Text;
            Boolean isValidate = Validate.isStoreValidate(store);
            if (isValidate) {
                DataProvider.Ins.DB.BookStores.Add(store);
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Tạo nhà sách thành công, Mã nhà sách là: " + store.Id, "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
