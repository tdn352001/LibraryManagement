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
    using System;
    using System.Collections.Generic;
    
    public partial class DetailImport
    {
        public int IdImport { get; set; }
        public int IdBook { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> PriceIn { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual ImportBook ImportBook { get; set; }
    }
}