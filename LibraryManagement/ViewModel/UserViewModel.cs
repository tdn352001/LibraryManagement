using LibraryManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        private ObservableCollection<User> _UserList;
        public ObservableCollection<User> UserList { get => _UserList; set { _UserList = value; OnPropertyChanged(); } }
        public UserViewModel()
        {
            UserList=new ObservableCollection<User>(DataProvider.Ins.DB.Users.ToList());
        }

    }
}
