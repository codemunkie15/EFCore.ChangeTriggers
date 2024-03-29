﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestHarness;

#nullable disable

namespace TestHarness.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20230804211037_1")]
    partial class _1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PermissionUser", b =>
                {
                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionsId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionsSubId")
                        .HasColumnType("int");

                    b.HasKey("UsersId", "PermissionsId", "PermissionsSubId");

                    b.HasIndex("PermissionsId", "PermissionsSubId");

                    b.ToTable("PermissionUser");
                });

            modelBuilder.Entity("TestHarness.DbModels.PaymentMethods.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("TestHarness.DbModels.Permissions.Permission", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("SubId")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid>("Reference")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id", "SubId");

                    b.ToTable("Permissions", null, t =>
                        {
                            t.HasTrigger("ChangeTrigger");
                        });

                    b
                        .HasAnnotation("ChangeTriggers:ChangeEntityTypeName", "TestHarness.DbModels.Permissions.PermissionChange")
                        .HasAnnotation("ChangeTriggers:Use", true);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            SubId = 1,
                            Enabled = false,
                            Name = "Permission 1",
                            Order = 0,
                            Reference = new Guid("00000000-0000-0000-0000-000000000000")
                        });
                });

            modelBuilder.Entity("TestHarness.DbModels.Permissions.PermissionChange", b =>
                {
                    b.Property<int>("ChangeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChangeId"));

                    b.Property<DateTimeOffset>("ChangedAt")
                        .HasColumnType("datetimeoffset")
                        .HasAnnotation("ChangeTriggers:IsChangedAtColumn", true);

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OperationType")
                        .HasColumnType("int")
                        .HasColumnName("OperationTypeId")
                        .HasAnnotation("ChangeTriggers:IsOperationTypeColumn", true);

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<Guid>("Reference")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SubId")
                        .HasColumnType("int");

                    b.HasKey("ChangeId");

                    b.HasIndex("Id", "SubId");

                    b.ToTable("PermissionChanges", (string)null);

                    b.HasAnnotation("ChangeTriggers:TrackedEntityTypeName", "TestHarness.DbModels.Permissions.Permission");
                });

            modelBuilder.Entity("TestHarness.DbModels.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PrimaryPaymentMethodId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryPaymentMethodId");

                    b.ToTable("Users", null, t =>
                        {
                            t.HasTrigger("ChangeTrigger")
                                .HasDatabaseName("ChangeTrigger1");
                        });

                    b
                        .HasAnnotation("ChangeTriggers:ChangeEntityTypeName", "TestHarness.DbModels.Users.UserChange")
                        .HasAnnotation("ChangeTriggers:Use", true);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = "01/01/2000",
                            Name = "Admin"
                        });
                });

            modelBuilder.Entity("TestHarness.DbModels.Users.UserChange", b =>
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

                    b.Property<string>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OperationType")
                        .HasColumnType("int")
                        .HasColumnName("OperationTypeId")
                        .HasAnnotation("ChangeTriggers:IsOperationTypeColumn", true);

                    b.Property<int?>("PrimaryPaymentMethodId")
                        .HasColumnType("int");

                    b.HasKey("ChangeId");

                    b.HasIndex("ChangedById");

                    b.HasIndex("Id");

                    b.HasIndex("PrimaryPaymentMethodId");

                    b.ToTable("UserChanges", (string)null);

                    b.HasAnnotation("ChangeTriggers:TrackedEntityTypeName", "TestHarness.DbModels.Users.User");
                });

            modelBuilder.Entity("PermissionUser", b =>
                {
                    b.HasOne("TestHarness.DbModels.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TestHarness.DbModels.Permissions.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId", "PermissionsSubId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestHarness.DbModels.PaymentMethods.PaymentMethod", b =>
                {
                    b.HasOne("TestHarness.DbModels.Users.User", "User")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TestHarness.DbModels.Permissions.PermissionChange", b =>
                {
                    b.HasOne("TestHarness.DbModels.Permissions.Permission", "TrackedEntity")
                        .WithMany("Changes")
                        .HasForeignKey("Id", "SubId")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true);

                    b.Navigation("TrackedEntity");
                });

            modelBuilder.Entity("TestHarness.DbModels.Users.User", b =>
                {
                    b.HasOne("TestHarness.DbModels.PaymentMethods.PaymentMethod", "PrimaryPaymentMethod")
                        .WithMany()
                        .HasForeignKey("PrimaryPaymentMethodId");

                    b.Navigation("PrimaryPaymentMethod");
                });

            modelBuilder.Entity("TestHarness.DbModels.Users.UserChange", b =>
                {
                    b.HasOne("TestHarness.DbModels.Users.User", "ChangedBy")
                        .WithMany()
                        .HasForeignKey("ChangedById")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true)
                        .HasAnnotation("ChangeTriggers:IsChangedByColumn", true);

                    b.HasOne("TestHarness.DbModels.Users.User", "TrackedEntity")
                        .WithMany("Changes")
                        .HasForeignKey("Id")
                        .HasAnnotation("ChangeTriggers:HasNoCheckConstraint", true);

                    b.HasOne("TestHarness.DbModels.PaymentMethods.PaymentMethod", "PrimaryPaymentMethod")
                        .WithMany()
                        .HasForeignKey("PrimaryPaymentMethodId");

                    b.Navigation("ChangedBy");

                    b.Navigation("PrimaryPaymentMethod");

                    b.Navigation("TrackedEntity");
                });

            modelBuilder.Entity("TestHarness.DbModels.Permissions.Permission", b =>
                {
                    b.Navigation("Changes");
                });

            modelBuilder.Entity("TestHarness.DbModels.Users.User", b =>
                {
                    b.Navigation("Changes");

                    b.Navigation("PaymentMethods");
                });
#pragma warning restore 612, 618
        }
    }
}
