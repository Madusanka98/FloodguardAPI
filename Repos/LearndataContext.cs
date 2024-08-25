using System;
using System.Collections.Generic;
using LearnAPI.Modal;
using LearnAPI.Repos.Models;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using FloodguardAPI.Repos.Models;

namespace LearnAPI.Repos;

public partial class LearndataContext : DbContext
{
    public LearndataContext()
    {
    }

    public LearndataContext(DbContextOptions<LearndataContext> options)
        : base(options)
    {
    }


    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblOtpManager> TblOtpManagers { get; set; }


    public virtual DbSet<TblPwdManger> TblPwdMangers { get; set; }

    public virtual DbSet<TblRefreshtoken> TblRefreshtokens { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRolepermission> TblRolepermissions { get; set; }


    public virtual DbSet<TblUser> TblUsers { get; set; }
    public virtual DbSet<TblHistoryData> TblHistoryDatas { get; set; }
    public virtual DbSet<TblRiverStation> TblRiverStations { get; set; }
    public virtual DbSet<TblRiver> TblRivers { get; set; }
    public virtual DbSet<TblRiverStationUsers> TblRiverStationUsers { get; set; }

    // Add Hangfire job and state tables
    //public virtual DbSet<Hangfire.Job> HangfireJobs { get; set; }
    //public virtual DbSet<Hangfire.State> HangfireStates { get; set; }

    
    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblTempuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tbl_tempuser1");
        });

        OnModelCreatingPartial(modelBuilder);
    }*/

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
