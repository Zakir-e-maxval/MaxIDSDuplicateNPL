//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuplicateNPL_Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_npl_duplicate_deletedLogs
    {
        public int Id { get; set; }
        public Nullable<int> PrimaryRecord { get; set; }
        public Nullable<int> SecondaryRecord { get; set; }
        public Nullable<int> SourceRecord { get; set; }
        public Nullable<int> PrivatePairId { get; set; }
        public Nullable<int> InputId { get; set; }
        public string Source { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
