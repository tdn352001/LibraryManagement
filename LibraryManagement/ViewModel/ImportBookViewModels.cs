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

    public class ImportBookViewModels : BaseViewModel {
        private ImportBookWindow window;
        private long _totalPrice { get; set; }
        public long totalPrice {
            get => _totalPrice;
            set {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        private BookStore _store { get; set; }
        public BookStore store { 
            get => _store;
            set { 
                _store = value; 
                OnPropertyChanged(); 
            } 
        }

        private ObservableCollection<DetailImport> _detailImports { get; set; }
        public ObservableCollection<DetailImport> detailImports { get => _detailImports; set { _detailImports = value; OnPropertyChanged(); } }

        private DetailImport _detailImport { get; set; }
        public DetailImport detailImport {
            get => _detailImport;
            set {
                _detailImport = value;
                OnPropertyChanged();
            }
        }

        public ImportBook importBook;

        public ICommand LoadedCommand { get; set; }
        public ICommand AddBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommamd { get; set; }
        public ImportBookViewModels() {
            InitCommand();
            InitVariables();
        }

        private void InitVariables() {
            detailImports = new ObservableCollection<DetailImport>();
            totalPrice = 0;
        }

        private void InitCommand() {
            LoadedCommand = new RelayCommand<Window>((p) => { return p != null; }, (p) => this.window = (ImportBookWindow)p);
            AddBookCommand = new RelayCommand<Object>((p) => { return true; }, (p) => AddBook());
            DeleteBookCommand = new RelayCommand<Object>((p) => { return true; }, (p) => DeleteBook());
            SaveCommand = new RelayCommand<Object>((p) => { return true; }, (p) => SaveImport());
            CancelCommamd = new RelayCommand<Object>((p) => { return true; }, (p) => CancelImport());
        }

        public void AddBook() {
            AddBookImportWindow window = new AddBookImportWindow();
            window.ShowDialog();
            if (window.isAdd) {

                int position = FindDetail(window.detailImport);
                if(position == -1) {
                    detailImports.Add(window.detailImport);
                    MessageBox.Show("Thêm thành công", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else {
                    MessageBox.Show("Sách này đã được thêm trước đó. Vui lòng cập nhật lại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                try {
                    int? price = window.detailImport.Quantity * window.detailImport.PriceIn; 
                    totalPrice += (long)price;
                } 
                catch (Exception) { }
            }
        }
        public void DeleteBook() {
            if(detailImport != null) {
                int? price = detailImport.Quantity * detailImport.PriceIn;
                totalPrice -= (long)price;
                detailImports.Remove(detailImport);
                detailImport = null;

            }
        }

        private void SaveImport() {
            if (this.isValidate()) {
                importBook = new ImportBook();
                importBook.BookStore = store;
                importBook.IdBookStore = store.Id;
                importBook.ImportDate = DateTime.Now;
                importBook.TotalPrice = (Int32)totalPrice;
                DataProvider.Ins.DB.ImportBooks.Add(importBook);
                DataProvider.Ins.DB.SaveChanges();

                foreach(var detail in detailImports) {
                    detail.IdImport = importBook.Id;
                    DataProvider.Ins.DB.DetailImports.Add(detail);

                    var book = DataProvider.Ins.DB.Books.Where(x => x.Id == detail.IdBook).SingleOrDefault();
                    int bookQuantity = book.Quantity != null ? (int)book.Quantity : 0;
                    int newQuantity = bookQuantity + (int)detail.Quantity;
                    book.Quantity = newQuantity;
                    DataProvider.Ins.DB.SaveChanges();
                }
                MessageBox.Show("Thêm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                if (window != null)
                    window.Close();
            }
        }

        private void CancelImport() {
            if (window != null)
                window.Close();
        }

        public void setStore(BookStore bookStore) {
            this.store = bookStore;
        }

        private int FindDetail(DetailImport detailImport) {
            for (int i = 0; i < detailImports.Count(); i++) {
                if(detailImports[i].IdBook == detailImport.IdBook) {
                    return i;
                }
            }

            return -1;
        }

        private Boolean isValidate() {
            if (store == null) {
                MessageBox.Show("Không tìm thấy nhà sách!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if(detailImports.Count == 0) {
                MessageBox.Show("Không tìm thấy thông tin sách nhập!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
