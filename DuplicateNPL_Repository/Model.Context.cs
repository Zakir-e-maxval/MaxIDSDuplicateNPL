﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class maxids_duplicateNPL_v4Entities : DbContext
    {
        public maxids_duplicateNPL_v4Entities()
            : base("name=maxids_duplicateNPL_v4Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_npl_document_name_formats> tbl_npl_document_name_formats { get; set; }
        public virtual DbSet<tbl_npl_duplicate_config> tbl_npl_duplicate_config { get; set; }
        public virtual DbSet<tbl_npl_duplicate_deletedLogs> tbl_npl_duplicate_deletedLogs { get; set; }
        public virtual DbSet<tbl_npl_duplicate_MergedLogs> tbl_npl_duplicate_MergedLogs { get; set; }
        public virtual DbSet<tbl_npl_duplicate_Score> tbl_npl_duplicate_Score { get; set; }
        public virtual DbSet<tbl_npl_keyword_format_mapping> tbl_npl_keyword_format_mapping { get; set; }
        public virtual DbSet<tbl_npl_keyword_format_mapping_history> tbl_npl_keyword_format_mapping_history { get; set; }
        public virtual DbSet<tbl_NPL_Parsed_Results> tbl_NPL_Parsed_Results { get; set; }
        public virtual DbSet<tbl_pair_references> tbl_pair_references { get; set; }
        public virtual DbSet<tbl_pair_references_FlagComments> tbl_pair_references_FlagComments { get; set; }
        public virtual DbSet<tbl_pair_references_header> tbl_pair_references_header { get; set; }
        public virtual DbSet<tbl_pair_request> tbl_pair_request { get; set; }
        public virtual DbSet<tbl_pair_request_status> tbl_pair_request_status { get; set; }
        public virtual DbSet<tbl_pair_request_tracking> tbl_pair_request_tracking { get; set; }
        public virtual DbSet<tbl_private_pair> tbl_private_pair { get; set; }
        public virtual DbSet<tbl_record> tbl_record { get; set; }
        public virtual DbSet<tbl_record_journals> tbl_record_journals { get; set; }
        public virtual DbSet<tbl_record_others> tbl_record_others { get; set; }
        public virtual DbSet<tbl_pair_references_ExaminerFlag> tbl_pair_references_ExaminerFlag { get; set; }
    }
}