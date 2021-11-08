using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class HistoryStoreViewModel : BaseViewModel
    {
        private BookStore _storeItem;
        public BookStore storeItem 
        { 
            get => _storeItem; 
            set { _storeItem = value; OnPropertyChanged(); } 
        }

        private ObservableCollection<ImportBook> _ImportList { get; set; }
        public ObservableCollection<ImportBook> ImportList { get => _ImportList; set { _ImportList = value; OnPropertyChanged(); } }

        private ImportBook _SelectedItem;
        public ImportBook SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;

                OnPropertyChanged();
            }
        }


        public ICommand DetailCommand { get; set; }


        public HistoryStoreViewModel(BookStore bookStore)
        {
            storeItem = bookStore;
            ImportList = new ObservableCollection<ImportBook>(bookStore.ImportBooks);


            DetailCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) =>
                                                              {
                                                                  MessageBox.Show("Hello");
                                                              });
        }
    }
}
