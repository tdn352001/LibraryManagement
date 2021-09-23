using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LibrabyManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private int _Screen = 2;
        public int Screen
        {
            get => _Screen; set { _Screen = value; OnPropertyChanged(); }
        }
        public ICommand HomeCommand { get; set; }
        public ICommand StoreCommand { get; set; }
        public ICommand BookCommand { get; set; }
        public ICommand ReaderCommand { get; set; }
        public ICommand FeeCommand { get; set; }
        public MainViewModel() 
        {

            // Click vào nút Home
            HomeCommand = new RelayCommand<Object>((p) => { return true; },
                                                               (p) => {
                                                                   Screen = 1;
                                                               });

            // Click vào nút Store
            StoreCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) => {
                                                                  Screen = 2;
                                                              });

            // Click vào nút Book
            BookCommand = new RelayCommand<Object>((p) => { return true; },
                                                              (p) => {
                                                                  Screen = 3;
                                                              });
            // Click vào nút Reader
            ReaderCommand = new RelayCommand<Object>((p) => { return true; },
                                                               (p) => {
                                                                   Screen = 4;
                                                               });
            // Click vào nút Fee
            FeeCommand = new RelayCommand<Object>((p) => { return true; },
                                                               (p) => {
                                                                   Screen = 5;
                                                               });
        }
    }
}
