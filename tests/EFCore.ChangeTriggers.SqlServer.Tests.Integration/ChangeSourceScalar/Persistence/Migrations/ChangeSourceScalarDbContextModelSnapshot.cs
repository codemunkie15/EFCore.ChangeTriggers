﻿// <auto-generated />
using System;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence.Migrations
{
    [DbContext(typeof(ChangeSourceScalarDbContext))]
    partial class ChangeSourceScalarDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSourceScalarUser", b =>
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
                        .HasAnnotation("ChangeTriggers:ChangeEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSourceScalarUserChange")
                        .HasAnnotation("ChangeTriggers:HasChangeTrigger", true)
                        .HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSourceScalarUserChange", b =>
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

                    b.HasIndex("Id");

                    b.ToTable("TestUserChanges");

                    b
                        .HasAnnotation("ChangeTriggers:ChangeSourceClrTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSource")
                        .HasAnnotation("ChangeTriggers:IsChangeTable", true)
                        .HasAnnotation("ChangeTriggers:TrackedEntityTypeName", "EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSourceScalarUser");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSourceScalarUserChange", b =>
                {
                    b.HasOne("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSourceScalarUser", "TrackedEntity")
                        .WithMany("Changes")
                        .HasForeignKey("Id")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true);

                    b.Navigation("TrackedEntity");
                });

            modelBuilder.Entity("EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain.ChangeSourceScalarUser", b =>
                {
                    b.Navigation("Changes");
                });
#pragma warning restore 612, 618
        }
    }
}
