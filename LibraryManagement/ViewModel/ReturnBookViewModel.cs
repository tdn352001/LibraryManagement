using LibraryManagement.Model;
using LibraryManagement.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.ViewModel {
    public class ReturnBookViewModel : BaseViewModel {

        private ReturnBookWindow window;

        private string _selectedName { get; set; }
        public string selectedName { get => _selectedName; set { _selectedName = value; OnPropertyChanged(); } }

        private HistoryBook _selectedItem { get; set; }
        public HistoryBook selectedItem {
            get => _selectedItem;
            set {
                _selectedItem = value;
                OnPropertyChanged();
                if (value != null) {
                    if (value.Book != null)
                        selectedName = value.Book.Name;
                }
                    
                else {
                    selectedName = "Chưa chọn sách";
                }
                    
            }
        }

        private ObservableCollection<HistoryBook> _historyBooks { get; set; }
        public ObservableCollection<HistoryBook> historyBooks { get => _historyBooks; set { _historyBooks = value; OnPropertyChanged(); } }

        private User _user { get; set; }
        public User user {
            get => _user; 
            set { 
                _user = value;
                OnPropertyChanged(); 
                if(value != null) {
                    historyBooks = new ObservableCollection<HistoryBook>(value.HistoryBooks.Where(x => x.IdStatus == Constant.BOOKSTATE_BORROWING).Distinct());
                }
            }
        }


        public ICommand LoadedCommand { get; set; }
        public ICommand ReturnCommand { get; set; }

        public ReturnBookViewModel(User user) {
            this.user = user;

            LoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => window = (ReturnBookWindow)p);
            ReturnCommand = new RelayCommand<Window>((p) => { return selectedItem != null; }, (p) => ReturnBook());
        }

        private void ReturnBook() {
            var historyBook = DataProvider.Ins.DB.HistoryBooks.Where(x => x.Id == selectedItem.Id).SingleOrDefault();
            if(historyBook != null) {
                historyBook.IdStatus = Constant.BOOKSTATE_RETURNED;
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Đã Lưu", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                historyBooks.Remove(selectedItem);
                selectedItem = null;
                if(historyBooks.Count() == 0) {
                    if (window != null)
                        window.Close();
                }
            }
        }
    }
}
