﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TND.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TNDEntities : DbContext
    {
        public TNDEntities()
            : base("name=TNDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Complain> Complains { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Req_Revert_MonthEnd> Req_Revert_MonthEnd { get; set; }
        public virtual DbSet<Month_End> Month_End { get; set; }
        public virtual DbSet<Temp_CPP> Temp_CPP { get; set; }
        public virtual DbSet<Upload_Data> Upload_Data { get; set; }
        public virtual DbSet<Ss_Interface> Ss_Interface { get; set; }
        public virtual DbSet<Feeder_Master> Feeder_Master { get; set; }
        public virtual DbSet<Ss_Master> Ss_Master { get; set; }
        public virtual DbSet<Change_Over> Change_Over { get; set; }
        public virtual DbSet<TND_Cal> TND_Cal { get; set; }
        public virtual DbSet<ATandC_Cal> ATandC_Cal { get; set; }
    }
}
