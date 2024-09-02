﻿// <auto-generated />
using System;
using LearnAPI.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FloodguardAPI.Migrations
{
    [DbContext(typeof(LearndataContext))]
    partial class LearndataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FloodguardAPI.Repos.Models.TblRiverStationUsers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Isactive")
                        .HasColumnType("bit")
                        .HasColumnName("isactive");

                    b.Property<int>("RiverStationId")
                        .HasColumnType("int")
                        .HasColumnName("riverStationId");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userId");

                    b.HasKey("Id");

                    b.ToTable("tbl_riverStationUsers");
                });

            modelBuilder.Entity("LearnAPI.Modal.TblHistoryData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Date")
                        .HasColumnType("date");

                    b.Property<bool?>("Isactive")
                        .HasColumnType("bit")
                        .HasColumnName("isactive");

                    b.Property<double>("RainfallData")
                        .HasColumnType("float")
                        .HasColumnName("rainfallData");

                    b.Property<double>("RiverHeight")
                        .HasColumnType("float")
                        .HasColumnName("riverHeight");

                    b.Property<int>("RiverStationId")
                        .HasColumnType("int")
                        .HasColumnName("riverStationId");

                    b.HasKey("Id");

                    b.ToTable("tbl_historyData");
                });

            modelBuilder.Entity("LearnAPI.Modal.TblRiverStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("AlertLevel")
                        .HasColumnType("float")
                        .HasColumnName("alertLevel");

                    b.Property<bool?>("Isactive")
                        .HasColumnType("bit")
                        .HasColumnName("isactive");

                    b.Property<string>("Latitude")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("latitude");

                    b.Property<string>("Longitude")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("longitude");

                    b.Property<double>("MajorLevel")
                        .HasColumnType("float")
                        .HasColumnName("majorLevel");

                    b.Property<double>("MinorLevel")
                        .HasColumnType("float")
                        .HasColumnName("minorLevel");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<int>("RiverId")
                        .HasColumnType("int")
                        .HasColumnName("riverId");

                    b.HasKey("Id");

                    b.ToTable("tbl_riverStation");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblMenu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("name");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("tbl_menu");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblOtpManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Createddate")
                        .HasColumnType("datetime")
                        .HasColumnName("createddate");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime")
                        .HasColumnName("expiration");

                    b.Property<string>("Otptext")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("otptext");

                    b.Property<string>("Otptype")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("otptype");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("tbl_otpManager");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblPwdManger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ModifyDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("tbl_pwdManger");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblRefreshtoken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Refreshtoken")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("refreshtoken");

                    b.Property<string>("Tokenid")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("tokenid");

                    b.Property<string>("Userid")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("userid");

                    b.HasKey("Id");

                    b.ToTable("tbl_refreshtoken");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblRiver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Isactive")
                        .HasColumnType("bit")
                        .HasColumnName("isactive");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("tbl_river");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("name");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("tbl_role");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblRolepermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Haveadd")
                        .HasColumnType("bit")
                        .HasColumnName("haveadd");

                    b.Property<bool>("Havedelete")
                        .HasColumnType("bit")
                        .HasColumnName("havedelete");

                    b.Property<bool>("Haveedit")
                        .HasColumnType("bit")
                        .HasColumnName("haveedit");

                    b.Property<bool>("Haveview")
                        .HasColumnType("bit")
                        .HasColumnName("haveview");

                    b.Property<string>("Menucode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("menucode");

                    b.Property<string>("Userrole")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("userrole");

                    b.HasKey("Id");

                    b.ToTable("tbl_rolepermission");
                });

            modelBuilder.Entity("LearnAPI.Repos.Models.TblUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("email");

                    b.Property<int?>("Failattempt")
                        .HasColumnType("int")
                        .HasColumnName("failattempt");

                    b.Property<bool?>("Isactive")
                        .HasColumnType("bit")
                        .HasColumnName("isactive");

                    b.Property<bool?>("Islocked")
                        .HasColumnType("bit")
                        .HasColumnName("islocked");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("phone");

                    b.Property<string>("Role")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("tbl_user");
                });
#pragma warning restore 612, 618
        }
    }
}
