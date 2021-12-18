using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SelectBookWindow.xaml
    /// </summary>
    public partial class SelectBookWindow : Window {

        public Book selectedItem;
        public SelectBookWindow() {
            InitializeComponent();
            selectedItem = null;
            bookList.ItemsSource = DataProvider.Ins.DB.Books.ToList();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e) {
            DisplayResultSearch(searchBox.Text);
        }

        private void bookList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedItem = (Book)bookList.SelectedItem;
            this.Close();
        }

        private void DisplayResultSearch(string keyWord) {
            if (String.IsNullOrWhiteSpace(keyWord)) {
                bookList.ItemsSource = new ObservableCollection<Book>(DataProvider.Ins.DB.Books);
                return;
            }
            else {
                bookList.ItemsSource = new ObservableCollection<Book>(DataProvider.Ins.DB.Books.Where(x => x.Name.ToString().ToLower().Contains(keyWord.ToLower())));
            }
        }
    }
}
