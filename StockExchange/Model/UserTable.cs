//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockExchange.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserTable
    {
        public int usr_id { get; set; }
        public string usr_name { get; set; }
        public string usr_sname { get; set; }
        public string usr_login { get; set; }
        public string usr_pass { get; set; }
        public Nullable<System.DateTime> usr_date { get; set; }
        public Nullable<bool> usr_aktywny { get; set; }
        public Nullable<int> wal_id { get; set; }
    
        public virtual Wallet Wallet { get; set; }
    }
}
