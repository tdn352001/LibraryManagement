using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LibraryManagement.ViewModel
{
    public class StoreViewModel : BaseViewModel
    {
        
        private ObservableCollection<BookStore> _StoreList { get; set; }
        public ObservableCollection<BookStore> StoreList { get => _StoreList; set { _StoreList = value; OnPropertyChanged(); } }

        public StoreViewModel()
        {
            
        }
        public StoreViewModel(LibraryManagementEntities db)
        {

        }
    }
}
