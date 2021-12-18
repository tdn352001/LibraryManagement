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

namespace LibraryManagement.Windows {
    /// <summary>
    /// Interaction logic for ImportBookWindow.xaml
    /// </summary>
    public partial class ImportBookWindow : Window {
        ImportBookViewModels viewModel;
        public ImportBookWindow() {
            InitializeComponent();
            viewModel = new ImportBookViewModels();
            this.DataContext = viewModel;
        }

        public ImportBookWindow(BookStore store) {
            InitializeComponent();
            viewModel = new ImportBookViewModels();
            tbIdStore.Text = store.Id.ToString();
            this.DataContext = viewModel;
        }

        private void tbIdStore_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void tbIdStore_TextChanged(object sender, TextChangedEventArgs e) {
            String idStore = tbIdStore.Text.ToString();
            try {
                int id = int.Parse(idStore);
                BookStore store = DataProvider.Ins.DB.BookStores.Where(x => x.Id == id).SingleOrDefault();
                viewModel.setStore(store);
            } 
            catch (Exception) {

            }
        }

        private void addStoreBtn_Click(object sender, RoutedEventArgs e) {
            AddStoreWindow window = new AddStoreWindow();
            window.ShowDialog();
        }

        private void selectStoreBtn_Click(object sender, RoutedEventArgs e) {
            SelectStoreWindow window = new SelectStoreWindow();
            window.ShowDialog();
            if(window.selectedItem != null) {
                viewModel.setStore(window.selectedItem);
            }
        }
    }
}
