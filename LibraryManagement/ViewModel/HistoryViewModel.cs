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
    public class HistoryViewModel : BaseViewModel {

        private HistoryWindow window;
        private User _user { get; set; }
        public User user {
            get => _user;
            set {
                _user = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<HistoryBook> _historyBooks { get; set; }
        public ObservableCollection<HistoryBook> historyBooks { get => _historyBooks; set { _historyBooks = value; OnPropertyChanged(); } }

        private ObservableCollection<DetailFee> _detailFees { get; set; }
        public ObservableCollection<DetailFee> detailFees { get => _detailFees; set { _detailFees = value; OnPropertyChanged(); } }

        public ICommand LoadedCommand { get; set; }

        public HistoryViewModel() {
            InitCommand();
            InitVariables();
            
        }

        public HistoryViewModel(User user) {
            this.user = user;
            InitCommand();
            InitVariables();
        }

        private void InitCommand() {
            LoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                window = (HistoryWindow)p;
                if (window != null && user == null) {
                    window.infoUser.Visibility = Visibility.Collapsed;
                }
            });
        }

        private void InitVariables() {
            if(user == null) {
                historyBooks = new ObservableCollection<HistoryBook>(DataProvider.Ins.DB.HistoryBooks.OrderByDescending(x => x.Id));
                detailFees = new ObservableCollection<DetailFee>(DataProvider.Ins.DB.DetailFees.OrderByDescending(x => x.Id));
                HistoryBook historyBook = historyBooks[0];
            }
            else {
                historyBooks = new ObservableCollection<HistoryBook>(DataProvider.Ins.DB.HistoryBooks.Where(x => x.IdUser == user.Id).OrderByDescending(x=> x.Id));
                detailFees = new ObservableCollection<DetailFee>(DataProvider.Ins.DB.DetailFees.Where(x => x.IdUser == user.Id).OrderByDescending(x => x.Id));
            }
        }
    }
}
