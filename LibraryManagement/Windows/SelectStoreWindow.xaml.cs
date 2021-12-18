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
    /// Interaction logic for SelectStoreWindow.xaml
    /// </summary>
    public partial class SelectStoreWindow : Window {

        public BookStore selectedItem;
        public SelectStoreWindow() {
            InitializeComponent();
            selectedItem = null;
            storeList.ItemsSource = DataProvider.Ins.DB.BookStores.ToList();
        }

        private void storeList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedItem = (BookStore)storeList.SelectedItem;
            this.Close();
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e) {
            DisplayResultSearch(searchBox.Text);
        }

        private void DisplayResultSearch(string keyWord) {
            
            if (String.IsNullOrWhiteSpace(keyWord)) {
                storeList.ItemsSource = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores);
                return;
            }
            else {
                switch (typeSearch.SelectedIndex) {
                    case 0:
                        storeList.ItemsSource = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Name.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 1:
                        storeList.ItemsSource = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Address.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                    case 2:
                        storeList.ItemsSource = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores.Where(x => x.Phone.ToString().ToLower().Contains(keyWord.ToLower())));
                        break;
                }
            }
        }

        private void typeSearch_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            searchBox.Text = "";
        }
    }
}
