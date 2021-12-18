using LibraryManagement.Model;
using LibraryManagement.ViewModel;
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

namespace LibraryManagement.Windows
{
    /// <summary>
    /// Interaction logic for HistoryStore.xaml
    /// </summary>
    public partial class HistoryStore : Window
    {
        HistoryStoreViewModel viewModel;

        public HistoryStore()
        {
            InitializeComponent();
        }

        public HistoryStore(BookStore bookStore)
        {
            InitializeComponent();
            viewModel = new HistoryStoreViewModel(bookStore);
            this.DataContext = viewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Button button = sender as Button;
            try {
                String strDetailId = button.DataContext.ToString();
                int detailId = int.Parse(strDetailId);
                viewModel.viewDetailImport(detailId);
            }
            catch (Exception) { }
        }
    }
}
