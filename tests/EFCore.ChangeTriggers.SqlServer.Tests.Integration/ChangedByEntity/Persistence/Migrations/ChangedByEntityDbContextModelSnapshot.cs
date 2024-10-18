﻿// <auto-generated />
using System;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence.Migrations
{
    [DbContext(typeof(ChangedByEntityDbContext))]
    partial class ChangedByEntityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUser", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TestUsers", t =>
                        {
                            t.HasTrigger("ChangeTrigger");
                        });

                    b
                        .HasAnnotation("ChangeTriggers:ChangeEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUserChange")
                        .HasAnnotation("ChangeTriggers:HasChangeTrigger", true)
                        .HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUserChange", b =>
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

                    b
                        .HasAnnotation("ChangeTriggers:ChangedByClrTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUser")
                        .HasAnnotation("ChangeTriggers:IsChangeTable", true)
                        .HasAnnotation("ChangeTriggers:TrackedEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUser");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUserChange", b =>
                {
                    b.HasOne("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUser", "ChangedBy")
                        .WithMany()
                        .HasForeignKey("ChangedById")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true)
                        .HasAnnotation("ChangeTriggers:IsChangedByColumn", true);

                    b.HasOne("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUser", "TrackedEntity")
                        .WithMany("Changes")
                        .HasForeignKey("Id")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true);

                    b.Navigation("ChangedBy");

                    b.Navigation("TrackedEntity");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain.ChangedByEntityUser", b =>
                {
                    b.Navigation("Changes");
                });
#pragma warning restore 612, 618
        }
    }
}
