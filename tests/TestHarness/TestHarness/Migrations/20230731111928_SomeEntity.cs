using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestHarness.Migrations
{
    /// <inheritdoc />
    public partial class SomeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SomeEntityId",
                table: "Permissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SomeEntityId",
                table: "PermissionChanges",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SomeEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SomeEntity", x => x.Id);
                });

            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "CustomTriggerName_Permissions");

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Permissions",
                changeTableName: "PermissionChanges",
                triggerName: "CustomTriggerName_Permissions",
                trackedTablePrimaryKeyColumns: new[] { "Id", "SubId" },
                changeTableDataColumns: new[] { "Enabled", "Id", "Name", "Order", "Reference", "SomeEntityId", "SubId" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumns: new[] { "Id", "SubId" },
                keyValues: new object[] { 1, 1 },
                column: "SomeEntityId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_SomeEntityId",
                table: "Permissions",
                column: "SomeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionChanges_SomeEntityId",
                table: "PermissionChanges",
                column: "SomeEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissionChanges_SomeEntity_SomeEntityId",
                table: "PermissionChanges",
                column: "SomeEntityId",
                principalTable: "SomeEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_SomeEntity_SomeEntityId",
                table: "Permissions",
                column: "SomeEntityId",
                principalTable: "SomeEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissionChanges_SomeEntity_SomeEntityId",
                table: "PermissionChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_SomeEntity_SomeEntityId",
                table: "Permissions");

            migrationBuilder.DropTable(
                name: "SomeEntity");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_SomeEntityId",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_PermissionChanges_SomeEntityId",
                table: "PermissionChanges");

            migrationBuilder.DropColumn(
                name: "SomeEntityId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "SomeEntityId",
                table: "PermissionChanges");

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Permissions",
                changeTableName: "PermissionChanges",
                triggerName: "CustomTriggerName_Permissions",
                trackedTablePrimaryKeyColumns: new[] { "Id", "SubId" },
                changeTableDataColumns: new[] { "Enabled", "Id", "Name", "Order", "Reference", "SubId" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"));

            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "CustomTriggerName_Permissions");
        }
    }
}
