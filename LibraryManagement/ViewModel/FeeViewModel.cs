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

        public ICommand EditFeeCommand { get; set; }
        public ICommand FeeDeleteCommand { get; set; }



        private ObservableCollection<DetailFee> _DetailFeeList { get; set; }
        public ObservableCollection<DetailFee> DetailFeeList { get => _DetailFeeList; set { _DetailFeeList = value; OnPropertyChanged(); } }




        public FeeViewModel()
        {
            FeeList = new ObservableCollection<Fee>(DataProvider.Ins.DB.Fees);
            canEditFeeSelectedItem = CanEditFeeSelectedItem();

            // Edit Fee Commnad
            EditFeeCommand = new RelayCommand<Object>((p) => {
                return canEditFeeSelectedItem;
            },
            (p) =>
            {
                MessageBox.Show("Helllo");                                                   
            });

            // Delete Fee Command
            FeeDeleteCommand = new RelayCommand<Object>((p) => {
                return canEditFeeSelectedItem;
            },
            (p) =>
            {
                MessageBox.Show("Helllo");
            });
        }




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
            if(selectedYear == currentYear)
                if (currentMonth >= selectedMonth)
                    return false;

            return true;
        }
    }
}