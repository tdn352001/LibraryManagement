using LibrabyManagement.ViewModel;
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
    
    
    public class ControlBarViewModel : BaseViewModel
    {
        private bool _isMaximize = false;
        public bool isMaximize { get => _isMaximize; set { _isMaximize = value; OnPropertyChanged(); } }
        

        // Đóng cửa sổ
        public ICommand CloseWindowCommand { get; set; }

        // Bật Maximize
        public ICommand MaxWindowCommand { get; set; }

        //Bât Minimize
        public ICommand MinWindowCommand { get; set; }

        //Di chuyển cửa sổ
        public ICommand MoveWindowCommand { get; set; }


        // Contructor
        public ControlBarViewModel()
        {
           

            // Đóng cửa sổ
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                                                               (p) => {
                                                                   FrameworkElement window = GetWindownParent(p);
                                                                   var win = window as Window;
                                                                   if (win != null)
                                                                       win.Close();
                                                               });


            // Bật Maximize
            MaxWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                                                               (p) => {
                                                                   FrameworkElement window = GetWindownParent(p);
                                                                   var win = window as Window;
                                                                   if (win != null)
                                                                   {
                                                                       if (win.WindowState != WindowState.Maximized)
                                                                       { 
                                                                           win.WindowState = WindowState.Maximized;
                                                                           isMaximize = true;
                                                                       }
                                                                       else
                                                                       { 
                                                                           win.WindowState = WindowState.Normal;
                                                                           isMaximize = false;
                                                                       }
                                                                   }
                                                               });

            //Bât Minimize
            MinWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                                                               (p) => {
                                                                   FrameworkElement window = GetWindownParent(p);
                                                                   var win = window as Window;
                                                                   if (win != null)
                                                                       win.WindowState = WindowState.Minimized;
                                                               });
            //Di chuyển cửa sổ
            MoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; },
                                                               (p) => {
                                                                   FrameworkElement window = GetWindownParent(p);
                                                                   var win = window as Window;
                                                                   if (win != null)
                                                                       win.DragMove();
                                                               });
        }

        //Lấy cửa sổ cha
        FrameworkElement GetWindownParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
                parent = parent.Parent as FrameworkElement;

            return parent;
        }
    }
}
