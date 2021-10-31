//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagement.Model
{
    using LibraryManagement.ViewModel;
    using System;
    using System.Collections.Generic;
    
    public partial class User:BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.DetailFees = new HashSet<DetailFee>();
            this.HistoryBooks = new HashSet<HistoryBook>();
        }

        private int _Id;
        public int Id { get => _Id; set { _Id = value;OnPropertyChanged(); } }

        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        Nullable<System.DateTime> _BirthDay;
        public Nullable<System.DateTime> BirthDay { get => _BirthDay; set { _BirthDay = value;OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value;OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value;OnPropertyChanged(); } }

        Nullable<int> _IdStatus;
        public Nullable<int> IdStatus { get => _IdStatus; set { _IdStatus = value;OnPropertyChanged(); } }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        
        public virtual ICollection<DetailFee> DetailFees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryBook> HistoryBooks { get; set; }

        private UserStatu _UserStatu;
      
        public virtual UserStatu UserStatu { get => _UserStatu; set { _UserStatu = value;OnPropertyChanged(); } }
    }
}
