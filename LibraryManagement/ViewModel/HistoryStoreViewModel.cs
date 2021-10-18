using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModel
{
    public class HistoryStoreViewModel : BaseViewModel
    {
        private BookStore _storeItem;
        public BookStore storeItem { get => _storeItem; set { _storeItem = value; OnPropertyChanged(); } }

        private ObservableCollection<BookStore> _StoreList { get; set; }
        public ObservableCollection<BookStore> StoreList { get => _StoreList; set { _StoreList = value; OnPropertyChanged(); } }

        public HistoryStoreViewModel(BookStore bookStore)
        {
            storeItem = bookStore;
            StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores);
        }
    }
}
