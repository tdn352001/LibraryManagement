using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModel {
    public class DetailImortViewModel : BaseViewModel{


        private BookStore _store { get; set; }
        public BookStore store {
            get => _store;
            set {
                _store = value;
                OnPropertyChanged();
            }
        }

        private ImportBook _import { get; set; }
        public ImportBook import {
            get => _import;
            set {
                _import = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DetailImport> _detailImports { get; set; }
        public ObservableCollection<DetailImport> detailImports { get => _detailImports; set { _detailImports = value; OnPropertyChanged(); } }

        public DetailImortViewModel(ImportBook importBook) {
            if (importBook == null) 
                return;
            this.import = importBook;
            store = import.BookStore;
            detailImports = new ObservableCollection<DetailImport>(import.DetailImports);
        }
    }
}
