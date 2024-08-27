﻿// <auto-generated />
using System;
using Benchmarks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Benchmarks.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Benchmarks.DbModels.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Column1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column10")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column11")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column12")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column13")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column14")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column15")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column3")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column4")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column5")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column6")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column7")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column8")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column9")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users", null, t =>
                        {
                            t.HasTrigger("ChangeTrigger");
                        });

                    b
                        .HasAnnotation("ChangeTriggers:ChangeEntityTypeName", "Benchmarks.DbModels.Users.UserChange")
                        .HasAnnotation("ChangeTriggers:Use", true);
                });

            modelBuilder.Entity("Benchmarks.DbModels.Users.UserChange", b =>
                {
                    b.Property<int>("ChangeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChangeId"));

                    b.Property<int>("ChangeSource")
                        .HasColumnType("int")
                        .HasAnnotation("ChangeTriggers:IsChangeSourceColumn", true);

                    b.Property<DateTimeOffset>("ChangedAt")
                        .HasColumnType("datetimeoffset")
                        .HasAnnotation("ChangeTriggers:IsChangedAtColumn", true);

                    b.Property<int>("ChangedById")
                        .HasColumnType("int");

                    b.Property<string>("Column1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column10")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column11")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column12")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column13")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column14")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column15")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column3")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column4")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column5")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column6")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column7")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column8")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column9")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("OperationType")
                        .HasColumnType("int")
                        .HasColumnName("OperationTypeId")
                        .HasAnnotation("ChangeTriggers:IsOperationTypeColumn", true);

                    b.HasKey("ChangeId");

                    b.HasIndex("ChangedById");

                    b.HasIndex("Id");

                    b.ToTable("UserChanges", (string)null);

                    b.HasAnnotation("ChangeTriggers:TrackedEntityTypeName", "Benchmarks.DbModels.Users.User");
                });

            modelBuilder.Entity("Benchmarks.DbModels.Users.UserChange", b =>
                {
                    b.HasOne("Benchmarks.DbModels.Users.User", "ChangedBy")
                        .WithMany()
                        .HasForeignKey("ChangedById")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true)
                        .HasAnnotation("ChangeTriggers:IsChangedByColumn", true);

                    b.HasOne("Benchmarks.DbModels.Users.User", "TrackedEntity")
                        .WithMany("Changes")
                        .HasForeignKey("Id")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true);

                    b.Navigation("ChangedBy");

                    b.Navigation("TrackedEntity");
                });

            modelBuilder.Entity("Benchmarks.DbModels.Users.User", b =>
                {
                    b.Navigation("Changes");
                });
#pragma warning restore 612, 618
        }
    }
}
