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
    
    public partial class tbl_npl_keyword_format_mapping
    {
        public int MappingId { get; set; }
        public string Keyword { get; set; }
        public string Format { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Comments { get; set; }
    }
}