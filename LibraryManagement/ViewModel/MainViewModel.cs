using LibraryManagement.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private int _Screen = 5;
        public int Screen
        {
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

        public MainViewModel()
        {
            storeControl = new StoreControl();
            readerControl = new ReaderControl();
            bookControl = new BookControl();
            feeControl = new FeeControl();

            LoadedCommand = new RelayCommand<Window>((p) => { return true; },
                                                               (p) => {
                                                                   mainWindow = (MainWindow)p;
                                                                   Screen = 5;
                                                                   mainWindow.controlContainer.Children.Add(feeControl);
                                                                   mainWindow.Tag = "Quản lý thu phí";
                                                               });


            // Click vào nút Home
            HomeCommand = new RelayCommand<Grid>((p) => { return true; },
                                                               (p) => {
                                                                   Screen = 1;
                                                                   mainWindow.Tag = "Home";
                                                               });

            // Click vào nút Store
            StoreCommand = new RelayCommand<Grid>((p) => { return true; },
                                                              (p) => {
                                                                  if(Screen != 2)
                                                                  {
                                                                      Screen = 2;
                                                                      p.Children.Clear();
                                                                      p.Children.Add(storeControl);
                                                                      mainWindow.Tag = "Quản lý nhà sách";
                                                                  }
                                                              });

            // Click vào nút Book
            BookCommand = new RelayCommand<Grid>((p) => { return true; },
                                                              (p) => {
                                                                  if (Screen != 3)
                                                                  {
                                                                      Screen = 3;
                                                                      p.Children.Clear();
                                                                      p.Children.Add(bookControl);
                                                                      mainWindow.Tag = "Quản lý sách";
                                                                  }
                                                              });
            // Click vào nút Reader
            ReaderCommand = new RelayCommand<Grid>((p) => { return true; },
                                                               (p) => {
                                                                   if (Screen != 4)
                                                                   {
                                                                       Screen = 4;
                                                                       p.Children.Clear();
                                                                       p.Children.Add(readerControl);
                                                                       mainWindow.Tag = "Quản lý thành viên";
                                                                   }
                                                               });
            // Click vào nút Fee
            FeeCommand = new RelayCommand<Grid>((p) => { return true; },
                                                               (p) => {
                                                                   if (Screen != 5)
                                                                   {
                                                                       Screen = 5;
                                                                       p.Children.Clear();
                                                                       p.Children.Add(feeControl);
                                                                       mainWindow.Tag = "Quản lý thu phí";
                                                                   }
                                                               });

     

        }

    }
}
