using LibraryManagement.Model;
using LibraryManagement.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel {

    public class AddFeeViewModel : BaseViewModel {
        DateTime now;
        DateTime currentDate;
        AddFeeWindow addFeeWindow;

        private DetailFee _detailFee { get; set; }
        public DetailFee detailFee { get => _detailFee; set { _detailFee = value; OnPropertyChanged(); } }

        private String _idUser { get; set; }
        public String idUser { get => _idUser; set { _idUser = value; OnPropertyChanged(); } }
        private User _user { get; set; }
        public User user {
            get => _user;
            set {
                _user = value;
                if (detailFee != null)
                    detailFee.User = value;
                OnPropertyChanged();
            }
        }

        private Fee _fee { get; set; }
        public Fee fee {
            get => _fee;
            set {
                _fee = value;
                detailFee.Fee = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Fee> _FeeList { get; set; }
        public ObservableCollection<Fee> FeeList { get => _FeeList; set { _FeeList = value; OnPropertyChanged(); } }


        public ICommand LoadedCommand { get; set; }
        public ICommand UserIdChange { get; set; }
        public ICommand CancelAddDetailCommand { get; set; }
        public ICommand SaveAddDetailCommand { get; set; }

        public AddFeeViewModel() {
            InitCommand();
            InitVariables();
        }

        public AddFeeViewModel(User user) {
            InitVariables();
            InitCommand();
            this.user = user;
            if(user != null)
                idUser = user.Id.ToString();
        }

        private void InitCommand() {
            LoadedCommand = new RelayCommand<Window>((p) => { return true; }, (p) => addFeeWindow = (AddFeeWindow)p);

            SaveAddDetailCommand = new RelayCommand<Window>((p) => { return !CheckDetailFeeNull(); }, (p) => AddDetailFee());

            UserIdChange = new RelayCommand<TextBox>((p) => { return true; }, (p) => FindUser(p.Text));

            CancelAddDetailCommand = new RelayCommand<Object>((p) => { return true; }, (p) => addFeeWindow.Close());
        }

        private void InitVariables() {
            now = DateTime.Now;
            currentDate = new DateTime(now.Year, now.Month, 1);
            detailFee = new DetailFee();
            detailFee.PayDate = now;

            FeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees
                .Where(x => DateTime.Compare(currentDate, (DateTime)x.Interval) <= 0)
                .OrderByDescending(x => x.Id));
            fee = DataProvider.Ins.DB.Fees.Where(x => x.Interval.Value.Month == currentDate.Month && x.Interval.Value.Year == currentDate.Year).SingleOrDefault();
        }

        private void FindUser(string text) {
            try {
                int id = int.Parse(text);
                user = DataProvider.Ins.DB.Users.Where(x => x.Id == id).SingleOrDefault();
            }
            catch (Exception) {

            }
        }

        private Boolean CheckDetailFeeNull() {
            return detailFee == null || user == null || fee == null;
        }

        private Boolean isDetailFeeValidate() {
            int alreadyPay = DataProvider.Ins.DB.DetailFees.Where(x => x.IdUser == detailFee.User.Id && x.IdFee == detailFee.Fee.Id).Count();
            if (alreadyPay != 0) {
                MessageBox.Show("Thành viên đã đóng trước đó", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            return true;
        }


        private void AddDetailFee() {
            if (isDetailFeeValidate()) {
                DataProvider.Ins.DB.DetailFees.Add(detailFee);
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Hóa đơn đã được tạo!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                addFeeWindow.Close();
            }
        }

    }
}
