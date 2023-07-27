using System;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestHarness.Migrations
{
    /// <inheritdoc />
    public partial class NewPermissionColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Reference",
                table: "Permissions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "PermissionChanges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Reference",
                table: "PermissionChanges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "CustomTriggerName_Permissions");

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Permissions",
                changeTableName: "PermissionChanges",
                triggerName: "CustomTriggerName_Permissions",
                trackedTablePrimaryKeyColumns: new[] { "Id", "SubId" },
                changeTableDataColumns: new[] { "Id", "Name", "Order", "Reference", "SubId" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumns: new[] { "Id", "SubId" },
                keyValues: new object[] { 1, 1 },
                columns: new[] { "Order", "Reference" },
                values: new object[] { 0, new Guid("00000000-0000-0000-0000-000000000000") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "PermissionChanges");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "PermissionChanges");

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Permissions",
                changeTableName: "PermissionChanges",
                triggerName: "CustomTriggerName_Permissions",
                trackedTablePrimaryKeyColumns: new[] { "Id", "SubId" },
                changeTableDataColumns: new[] { "Id", "Name", "SubId" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"));

            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "CustomTriggerName_Permissions");
        }
    }
}
