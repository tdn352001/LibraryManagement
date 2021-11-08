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

        // Danh sách các nhà sách trong database
        private ObservableCollection<DetailFee> _FeeList { get; set; }
        public ObservableCollection<DetailFee> FeeList { get => _FeeList; set { _FeeList = value; OnPropertyChanged(); } }

        // nhà sách được chọn trên list view
        private DetailFee _SelectedItem;
        public DetailFee SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;

                OnPropertyChanged();
            }
        }

        // nhà sách được thêm
        private DetailFee _AddItem;
        public DetailFee AddItem
        {
            get => _AddItem;
            set
            {
                _AddItem = value;
                OnPropertyChanged();
            }
        }

        // kiểm tra thông tin đang hiển thị là của item được chọn hay của item muốn thêm
        private bool _AddLayoutVisible = false;
        public bool AddLayoutVisible { get => _AddLayoutVisible; set { _AddLayoutVisible = value; OnPropertyChanged(); } }

        // thứ tự của heading được chọn trong phân loại tìm kiếm trong commbo box
        private int _SearchHeading = 1;
        public int SearchHeading { get => _SearchHeading; set { _SearchHeading = value; OnPropertyChanged(); } }

        //Command xử lí khi click vào nút thêm (nút trên list view)
        public ICommand AddCommand { get; set; }

        // Command xử lí khi ấn vào nút lưu trong layout thêm item
        public ICommand SaveAddCommand { get; set; }
        // Command xử lí khi ấn vào nút hủy trong layout thêm item
        public ICommand CancelAddCommand { get; set; }

        // Command xử lí khi ấn vào nút lưu trong layout selected item
        public ICommand EditCommand { get; set; }
        // Command xử lí khi ấn vào nút xóa trong layout selected item
        public ICommand DeleteCommand { get; set; }
        // Command xử lí khi ấn vào nút tìm kiếm 
        public ICommand SearchCommand { get; set; }
        // Command xử lí khi ấn vào nút lịch sử nhập sách
        public ICommand ExportInfoCommand { get; set; }
        //Command xử lí khi key word thay đổi
        public ICommand KeyWordChangeCommand { get; set; }

        public FeeViewModel()
        {
            // lấy danh sách từ database
            FeeList = new ObservableCollection<DetailFee>(DataProvider.Ins.DB.DetailFees);
        }       
    }
}