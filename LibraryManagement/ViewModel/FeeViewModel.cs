using LibraryManagement.Model;
using LibraryManagement.Windows;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace LibraryManagement.ViewModel
{
    public class FeeViewModel : BaseViewModel
    {
        DateTime now;
        DateTime currentDate;


        // Tab1: Phần Fee
        private ObservableCollection<Fee> _FeeList { get; set; }
        public ObservableCollection<Fee> FeeList { get => _FeeList; set { _FeeList = value; OnPropertyChanged(); } }

        private Fee _FeeSelectedItem;
        public Fee FeeSelectedItem
        {
            get => _FeeSelectedItem;
            set
            {
                _FeeSelectedItem = value;
                canEditFeeSelectedItem = CanEditFeeSelectedItem();
                OnPropertyChanged();
            }
        }

        private Fee _AddFeeItem;
        public Fee AddFeeItem
        {
            get => _AddFeeItem;
            set
            {
                _AddFeeItem = value;
                OnPropertyChanged();
            }
        }

        private DateTime _AddFeeDate;
        public DateTime AddFeeDate
        {
            get => _AddFeeDate;
            set
            {
                DateTime selectedDate = new DateTime(value.Year, value.Month, 1);
                _AddFeeDate = selectedDate;
                if (AddFeeItem != null)
                    AddFeeItem.Interval = selectedDate;
                OnPropertyChanged();
            }
        }

        private bool _canEditFeeSelectedItem;
        public bool canEditFeeSelectedItem
        {
            get => _canEditFeeSelectedItem;
            set
            {
                _canEditFeeSelectedItem = value;
                OnPropertyChanged();
            }
        }

        // kiểm tra thông tin đang hiển thị là của item được chọn hay của item muốn thêm
        private bool _AddFeeLayoutVisible = false;
        public bool AddFeeLayoutVisible { get => _AddFeeLayoutVisible; set { _AddFeeLayoutVisible = value; OnPropertyChanged(); } }

        public ICommand AddFeeCommand { get; set; }
        public ICommand SaveAddFeeCommand { get; set; }
        public ICommand CancelAddFeeCommand { get; set; }
        public ICommand EditFeeCommand { get; set; }
        public ICommand DeleteFeeCommand { get; set; }



        // Tab2: Phần Detail
        private ObservableCollection<DetailFee> _DetailList { get; set; }
        public ObservableCollection<DetailFee> DetailList { get => _DetailList; set { _DetailList = value; OnPropertyChanged(); } }
        private Fee _FeeFilterSelected { get; set; }
        public Fee FeeFilterSelected
        {
            get => _FeeFilterSelected;
            set
            {
                _FeeFilterSelected = value;
                if (value != null)
                    DetailList = new ObservableCollection<DetailFee>(DataProvider.Ins.DB.DetailFees.Where(x => x.Fee.Interval == value.Interval));
                else
                    DetailList.Clear();
                OnPropertyChanged();
            }
        }

        private DetailFee _DetailSelectedItem;
        public DetailFee DetailSelectedItem
        {
            get => _DetailSelectedItem;
            set
            {
                _DetailSelectedItem = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Nguyên phần ADD NEW DETAIL FEE
        /// </summary>
        private DetailFee _AddDetailFeeItem { get; set; }
        public DetailFee AddDetailFeeItem { get => _AddDetailFeeItem; set { _AddDetailFeeItem = value; OnPropertyChanged(); } }

        private Fee _AddDetailFeeSelected { get; set; }
        public Fee AddDetailFeeSelected
        {
            get => _AddDetailFeeSelected;
            set
            {
                _AddDetailFeeSelected = value;
                AddDetailFeeItem.Fee = value;
                AddDetailFeeItem.IdFee = value == null ? 0 : value.Id;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Fee> _FutureFeeList { get; set; }
        public ObservableCollection<Fee> FutureFeeList { get => _FutureFeeList; set { _FutureFeeList = value; OnPropertyChanged(); } }
        private User _user { get; set; }
        public User user
        {
            get => _user;
            set
            {
                _user = value;
                AddDetailFeeItem.User = value;
                AddDetailFeeItem.IdUser = value == null ? 0 : value.Id;
                OnPropertyChanged();
            }
        }

        private String _idUser { get; set; }

        public String idUser
        {
            get => _idUser;
            set
            {
                _idUser = value;
                if (value == "") return;
                int userId = int.Parse(value);
                User selectedUser = DataProvider.Ins.DB.Users.Where(x => x.Id == userId).SingleOrDefault();
                user = selectedUser;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// End
        /// </summary>


        // kiểm tra thông tin đang hiển thị là của item được chọn hay của item muốn thêm
        private bool _AddDetailLayoutVisible = false;
        public bool AddDetailLayoutVisible { get => _AddDetailLayoutVisible; set { _AddDetailLayoutVisible = value; OnPropertyChanged(); } }




        public ICommand AddDetailCommand { get; set; }
        public ICommand CancelAddDetailCommand { get; set; }
        public ICommand SaveAddDetailCommand { get; set; }
        // Contructor
        public FeeViewModel()
        {
            now = DateTime.Now;
            currentDate = new DateTime(now.Year, now.Month, 1);
            FeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees.OrderByDescending(x=> x.Id));
            FutureFeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees.Where(x => DateTime.Compare(currentDate, (DateTime)x.Interval) <= 0));
            FeeFilterSelected = CurrentFeeMonth();
            canEditFeeSelectedItem = CanEditFeeSelectedItem();
            
            AddFeeCommand = new RelayCommand<Object>((p) => { return true; },
            (p) =>
            {
                AddFeeLayoutVisible = true;
                AddFeeItem = new Fee();
                DateTime now = DateTime.Now;
                DateTime nextMonth = now.AddMonths(1);
                AddFeeDate = new DateTime(nextMonth.Year, nextMonth.Month, 1);
                OnPropertyChanged("AddFeeItem");

            });


            CancelAddFeeCommand = new RelayCommand<Object>((p) => { return true; },
            (p) =>
            {
                AddFeeLayoutVisible = false;
                AddFeeItem = null;
            });


            // Save Add Fee Command
            SaveAddFeeCommand = new RelayCommand<Object>(
            (p) => {
                DateTime now = DateTime.Now;
                DateTime CurrentMonth = new DateTime(now.Year, now.Month, 1);
                return DateTime.Compare(CurrentMonth, AddFeeDate) < 0 && !CheckExistDateInDB();
            },
            (p) => {
                if (AddFeeItem.Money >= 0)
                {
                    AddFee();
                }
                else
                {
                    MessageBox.Show("Số tiền phải là số không âm", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            });

            // Edit Fee Commnad
            EditFeeCommand = new RelayCommand<Object>((p) =>
            {
                return canEditFeeSelectedItem;
            },
            (p) =>
            {
                if (FeeSelectedItem.Money >= 0)
                {
                    SaveChangeFee();
                }
                else
                {
                    MessageBox.Show("Số tiền phải là số không âm", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            // Delete Fee Command
            DeleteFeeCommand = new RelayCommand<Object>((p) =>
            {
                return canEditFeeSelectedItem;
            },
            (p) =>
            {
                DeleteFee();
            });


            AddDetailCommand = new RelayCommand<Object>((p) =>{ return true;},
           (p) =>
           {
               AddDetailLayoutVisible = true;
               AddDetailFeeItem = new DetailFee();
               AddDetailFeeItem.PayDate = now;
               OnPropertyChanged("AddDetailFeeItem");
           }); 


            CancelAddDetailCommand = new RelayCommand<Object>((p) =>{ return true;},
           (p) =>
           {
               AddDetailLayoutVisible = false;
               idUser = "";
               user = null;
               AddDetailFeeItem = null;
           });

            SaveAddDetailCommand = new RelayCommand<Object>(
                (p) => {
                    if (AddDetailFeeItem == null ||AddDetailFeeItem.Fee == null || AddDetailFeeItem.User == null)
                        return false;
                    return true; 
                },
              (p) =>
              {
                  SaveAddDetail();
              });

        }


        private Fee CurrentFeeMonth()
        {
            int currentYear = DateTime.UtcNow.Year;
            int currentMonth = DateTime.UtcNow.Month;
            Fee currentFee = DataProvider.Ins.DB.Fees.Where(x => x.Interval.Value.Month == currentMonth && x.Interval.Value.Year == currentYear).Single();
            return currentFee;
        }
      
        // Check Fee Selected Item Can Change
        private bool CanEditFeeSelectedItem()
        {
            if (FeeSelectedItem == null)
                return false;
            int currentYear = DateTime.UtcNow.Year;
            int selectedYear = FeeSelectedItem.Interval.Value.Year;

            if (selectedYear < currentYear)
                return false;

            int currentMonth = DateTime.UtcNow.Month;
            int selectedMonth = FeeSelectedItem.Interval.Value.Month;
            if (selectedYear == currentYear)
                if (currentMonth >= selectedMonth)
                    return false;

            return true;
        }

        private bool CheckExistDateInDB()
        {
            var countDate = DataProvider.Ins.DB.Fees.Where(x => DateTime.Compare(AddFeeDate, (DateTime)x.Interval) == 0).Count();
            return countDate > 0;
        }

        // Save Add Fee
        private void AddFee()
        {
            if(AddFeeItem != null)
            {
                DataProvider.Ins.DB.Fees.Add(AddFeeItem);
                DataProvider.Ins.DB.SaveChanges();
                FeeSelectedItem = AddFeeItem;
                FeeList.Insert(0, AddFeeItem);
                AddFeeItem = null;
                AddFeeLayoutVisible = false;
                MessageBox.Show("Thêm thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Save Change FeeItem
        private void SaveChangeFee()
        {
            var fee = DataProvider.Ins.DB.Fees.Where(x => x.Id == FeeSelectedItem.Id).SingleOrDefault();
            fee.Money = FeeSelectedItem.Money;
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Đã Lưu", "Lưu Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Delete Fee Item
        private void DeleteFee()
        {
            if (FeeSelectedItem == null)
                return;

            String title = "Xóa?";
            String message = "Hành động này không thể khôi phục, Bạn có chắc chắn?";
            MessageBoxResult dialogResult = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (dialogResult == MessageBoxResult.Yes)
            {
                DataProvider.Ins.DB.Fees.Remove(FeeSelectedItem);
                DataProvider.Ins.DB.SaveChanges();
                FeeList.Remove(FeeSelectedItem);
                String notifyTitle = "Thông báo";
                String notifyMessage = "Xóa thành công.";
                MessageBox.Show(notifyMessage, notifyTitle, MessageBoxButton.OK);
            }
        }
        

        private void SaveAddDetail()
        {
            int alreadyPay = DataProvider.Ins.DB.DetailFees.Where(x => x.IdUser == AddDetailFeeItem.User.Id && x.IdFee == AddDetailFeeItem.Fee.Id).Count();
            if(alreadyPay != 0)
            {
                MessageBox.Show("Thành viên đã đóng trước đó", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            DataProvider.Ins.DB.DetailFees.Add(AddDetailFeeItem);
            DataProvider.Ins.DB.SaveChanges();

            if(AddDetailFeeItem.Fee == CurrentFeeMonth())
            {
                DetailFee newDetail = DataProvider.Ins.DB.DetailFees.Where(x => x.Id == AddDetailFeeItem.Id).SingleOrDefault();
                DetailSelectedItem = newDetail;
                DetailList.Add(newDetail);
                int pos = DetailList.IndexOf(newDetail);
                MessageBox.Show(DetailList[pos].User.Name + ": Nộp tiền thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // RESET
            AddDetailLayoutVisible = false;
            idUser = "";
            user = null;
            AddDetailFeeItem = null;
        }
    }
}