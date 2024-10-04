﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence.Migrations
{
    [DbContext(typeof(ChangedByEntityDbContext))]
    [Migration("20241002215933_Initial")]
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

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUser", b =>
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
                        .HasAnnotation("ChangeTriggers:ChangeEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUserChange")
                        .HasAnnotation("ChangeTriggers:Use", true)
                        .HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUserChange", b =>
                {
                    b.Property<int>("ChangeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChangeId"));

                    b.Property<DateTimeOffset>("ChangedAt")
                        .HasColumnType("datetimeoffset")
                        .HasAnnotation("ChangeTriggers:IsChangedAtColumn", true);

                    b.Property<int>("ChangedById")
                        .HasColumnType("int");

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

                    b.HasIndex("ChangedById");

                    b.HasIndex("Id");

                    b.ToTable("TestUserChanges");

                    b.HasAnnotation("ChangeTriggers:TrackedEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUser");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUserChange", b =>
                {
                    b.HasOne("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUser", "ChangedBy")
                        .WithMany()
                        .HasForeignKey("ChangedById")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true)
                        .HasAnnotation("ChangeTriggers:IsChangedByColumn", true);

                    b.HasOne("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUser", "TrackedEntity")
                        .WithMany("Changes")
                        .HasForeignKey("Id")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true);

                    b.Navigation("ChangedBy");

                    b.Navigation("TrackedEntity");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.TestUser", b =>
                {
                    b.Navigation("Changes");
                });
#pragma warning restore 612, 618
        }
    }
}
