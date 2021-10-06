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
    public class StoreViewModel : BaseViewModel
    {
       
        private ObservableCollection<BookStore> _StoreList { get; set; }
        public ObservableCollection<BookStore> StoreList { get => _StoreList; set { _StoreList = value; OnPropertyChanged(); } }

        private BookStore _SelectedItem;
        public BookStore SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                
                OnPropertyChanged();
            }
        }

        private BookStore _AddItem;
        public BookStore AddItem
        {
            get => _AddItem;
            set
            {
                _AddItem = value;
                OnPropertyChanged();
            }
        }

        private bool _AddLayoutVisible = false;
        public bool AddLayoutVisible { get => _AddLayoutVisible; set { _AddLayoutVisible = value; OnPropertyChanged(); } }


        public ICommand AddCommand { get; set; }
        public ICommand SaveAddCommand { get; set; }
        public ICommand CancelAddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand HistoryStoreCommand { get; set; }
        public ICommand ExportInfoCommand { get; set; }

        public StoreViewModel()
        {
            StoreList = new ObservableCollection<BookStore>(DataProvider.Ins.DB.BookStores);

            EditCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) => {
                                                                  if(BookStoreValidate(SelectedItem))
                                                                    SaveChangeStore();
                                                              });

            AddCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) => {
                                                                  AddLayoutVisible = true;
                                                                  AddItem = new BookStore();
                                                                  DateTime now = DateTime.Now;
                                                                  AddItem.CoopDate = now;
                                                              });
            CancelAddCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) => {
                                                                 AddLayoutVisible = false;
                                                                 AddItem = null;
                                                             });
            SaveAddCommand = new RelayCommand<Object>((p) => { return true; },
                                                             (p) => {
                                                                 if (BookStoreValidate(AddItem))
                                                                     SaveAddStore();
                                                             });
        }



        private void SaveChangeStore()
        {
            if(SelectedItem != null)
            {
                var store = DataProvider.Ins.DB.BookStores.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                store.Name = SelectedItem.Name;
                store.Address = SelectedItem.Address;
                store.MoreInfo = SelectedItem.MoreInfo;
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Đã Lưu", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SaveAddStore()
        {
            if(AddItem != null)
            {
                DataProvider.Ins.DB.BookStores.Add(AddItem);
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem = AddItem;
                StoreList.Add(AddItem);
                AddItem = null;
                AddLayoutVisible = false;
            }
        }

        private Boolean BookStoreValidate(BookStore store)
        {
            if (store == null)
                return false;

            if (String.IsNullOrEmpty(store.Name))
                return false;

            if (String.IsNullOrEmpty(store.Address))
                return false;

            return true;
        }
      
    }
}
