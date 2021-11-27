using LibraryManagement.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel {
    public class MainViewModel : BaseViewModel {
        private int _Screen;
        public int Screen {
            get => _Screen;
            set { _Screen = value; OnPropertyChanged(); }
        }

        StoreControl storeControl;
        ReaderControl readerControl;
        BookControl bookControl;
        FeeControl feeControl;
        MainWindow mainWindow;

        public ICommand LoadedCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand StoreCommand { get; set; }
        public ICommand BookCommand { get; set; }
        public ICommand ReaderCommand { get; set; }
        public ICommand FeeCommand { get; set; }

        public MainViewModel() {

            LoadedCommand = new RelayCommand<Window>((p) => { return true; },
                                                               (p) => {
                                                                   mainWindow = (MainWindow)p;
                                                                   readerControl = new ReaderControl();
                                                                   mainWindow.controlContainer.Children.Add(readerControl);
                                                                   mainWindow.Tag = "Quản lý thành viên";
                                                                   Screen = 4;
                                                               });


            // Click vào nút Home
            HomeCommand = new RelayCommand<Grid>((p) => { return true; },
                                                               (p) => {
                                                                   mainWindow.Tag = "Home";
                                                                   ClearUserControl();
                                                                   Screen = 1;
                                                               });

            // Click vào nút Store
            StoreCommand = new RelayCommand<Grid>((p) => { return true; },
                                                              (p) => {
                                                                  if (Screen != 2) {
                                                                      storeControl = new StoreControl();
                                                                      p.Children.Clear();
                                                                      p.Children.Add(storeControl);
                                                                      mainWindow.Tag = "Quản lý nhà sách";
                                                                      ClearUserControl();
                                                                      Screen = 2;
                                                                  }
                                                              });

            // Click vào nút Book
            BookCommand = new RelayCommand<Grid>((p) => { return true; },
                                                              (p) => {
                                                                  if (Screen != 3) {
                                                                      bookControl = new BookControl();
                                                                      p.Children.Clear();
                                                                      p.Children.Add(bookControl);
                                                                      mainWindow.Tag = "Quản lý sách";
                                                                      ClearUserControl();
                                                                      Screen = 3;
                                                                  }
                                                              });
            // Click vào nút Reader
            ReaderCommand = new RelayCommand<Grid>((p) => { return true; },
                                                               (p) => {
                                                                   if (Screen != 4) {
                                                                       readerControl = new ReaderControl();
                                                                       p.Children.Clear();
                                                                       p.Children.Add(readerControl);
                                                                       mainWindow.Tag = "Quản lý thành viên";
                                                                       ClearUserControl();
                                                                       Screen = 4;

                                                                   }
                                                               });
            // Click vào nút Fee
            FeeCommand = new RelayCommand<Grid>((p) => { return true; },
                                                               (p) => {
                                                                   if (Screen != 5) {
                                                                       feeControl = new FeeControl();
                                                                       p.Children.Clear();
                                                                       p.Children.Add(feeControl);
                                                                       mainWindow.Tag = "Quản lý thu phí";
                                                                       ClearUserControl();
                                                                       Screen = 5;
                                                                   }
                                                               });


        }

        private void ClearUserControl() {
            switch (Screen) {
                case 1:
                    break;
                case 2:
                    storeControl = null;
                    break;
                case 3:
                    bookControl = null;
                    break;
                case 4:
                    readerControl = null;
                    break;
                case 5:
                    feeControl = null;
                    break;
            }
        }
    }
}
