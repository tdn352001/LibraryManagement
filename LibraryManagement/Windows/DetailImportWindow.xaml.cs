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

namespace LibraryManagement.Windows {
    /// <summary>
    /// Interaction logic for DetailImportWindow.xaml
    /// </summary>
    public partial class DetailImportWindow : Window {
        DetailImortViewModel viewModel;
        public DetailImportWindow() {
            InitializeComponent();
        }

        public DetailImportWindow(ImportBook import) {
            InitializeComponent();
            viewModel = new DetailImortViewModel(import);
            this.DataContext = viewModel;
        }
    }
}
