using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestHarness.Migrations
{
    /// <inheritdoc />
    public partial class PermissionEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "PermissionChanges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "CustomTriggerName_Permissions");

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Permissions",
                changeTableName: "PermissionChanges",
                triggerName: "CustomTriggerName_Permissions",
                trackedTablePrimaryKeyColumns: new[] { "Id", "SubId" },
                changeTableDataColumns: new[] { "Enabled", "Id", "Name", "Order", "Reference", "SubId" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumns: new[] { "Id", "SubId" },
                keyValues: new object[] { 1, 1 },
                column: "Enabled",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "PermissionChanges");

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Permissions",
                changeTableName: "PermissionChanges",
                triggerName: "CustomTriggerName_Permissions",
                trackedTablePrimaryKeyColumns: new[] { "Id", "SubId" },
                changeTableDataColumns: new[] { "Id", "Name", "Order", "Reference", "SubId" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"));

            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "CustomTriggerName_Permissions");
        }
    }
}
