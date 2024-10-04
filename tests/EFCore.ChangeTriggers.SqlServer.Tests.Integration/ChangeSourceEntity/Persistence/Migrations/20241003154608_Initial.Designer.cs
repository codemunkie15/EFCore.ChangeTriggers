﻿// <auto-generated />
using System;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence.Migrations
{
    [DbContext(typeof(ChangeSourceEntityDbContext))]
    [Migration("20241003154608_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChangeSources");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Migrations"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Tests"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Web API"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Console"
                        },
                        new
                        {
                            Id = 5,
                            Name = "SQL"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Mobile"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Public API"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Email Service"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Data Retention Service"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Maintenance"
                        });
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSourceEntityUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TestUsers", t =>
                        {
                            t.HasTrigger("ChangeTrigger");
                        });

                    b
                        .HasAnnotation("ChangeTriggers:ChangeEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSourceEntityUserChange")
                        .HasAnnotation("ChangeTriggers:Use", true)
                        .HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSourceEntityUserChange", b =>
                {
                    b.Property<int>("ChangeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChangeId"));

                    b.Property<int>("ChangeSourceId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ChangedAt")
                        .HasColumnType("datetimeoffset")
                        .HasAnnotation("ChangeTriggers:IsChangedAtColumn", true);

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("OperationType")
                        .HasColumnType("int")
                        .HasColumnName("OperationTypeId")
                        .HasAnnotation("ChangeTriggers:IsOperationTypeColumn", true);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChangeId");

                    b.HasIndex("ChangeSourceId");

                    b.HasIndex("Id");

                    b.ToTable("TestUserChanges");

                    b.HasAnnotation("ChangeTriggers:TrackedEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSourceEntityUser");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSourceEntityUserChange", b =>
                {
                    b.HasOne("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSource", "ChangeSource")
                        .WithMany()
                        .HasForeignKey("ChangeSourceId")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true)
                        .HasAnnotation("ChangeTriggers:IsChangeSourceColumn", true);

                    b.HasOne("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSourceEntityUser", "TrackedEntity")
                        .WithMany("Changes")
                        .HasForeignKey("Id")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true);

                    b.Navigation("ChangeSource");

                    b.Navigation("TrackedEntity");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain.ChangeSourceEntityUser", b =>
                {
                    b.Navigation("Changes");
                });
#pragma warning restore 612, 618
        }
    }
}