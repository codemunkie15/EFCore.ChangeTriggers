using System;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _01FullyFeatured.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionChanges",
                columns: table => new
                {
                    ChangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionChanges", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_PermissionChanges_Permissions_Id",
                        column: x => x.Id,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserChanges",
                columns: table => new
                {
                    ChangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangeSource = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangedById = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChanges", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_UserChanges_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserChanges_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.AddNoCheckConstraint(
                table: "PermissionChanges",
                constraint: "FK_PermissionChanges_Permissions_Id");

            migrationBuilder.AddNoCheckConstraint(
                table: "UserChanges",
                constraint: "FK_UserChanges_Users_ChangedById");

            migrationBuilder.AddNoCheckConstraint(
                table: "UserChanges",
                constraint: "FK_UserChanges_Users_Id");

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Permissions",
                changeTableName: "PermissionChanges",
                triggerName: "CustomTriggerName_Permissions",
                trackedTablePrimaryKeyColumns: new[] { "Id" },
                changeTableDataColumns: new[] { "Id", "Name" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"));

            migrationBuilder.CreateChangeTrackingTrigger(
                trackedTableName: "Users",
                changeTableName: "UserChanges",
                triggerName: "Users_ChangeTracking",
                trackedTablePrimaryKeyColumns: new[] { "Id" },
                changeTableDataColumns: new[] { "DateOfBirth", "Id", "Name" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"),
                changeSourceColumn: new ChangeContextColumn("ChangeSource", "int"),
                changedByColumn: new ChangeContextColumn("ChangedById", "int"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateOfBirth", "Name" },
                values: new object[] { 1, "01/01/2000", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionChanges_Id",
                table: "PermissionChanges",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserChanges_ChangedById",
                table: "UserChanges",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserChanges_Id",
                table: "UserChanges",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "CustomTriggerName_Permissions");

            migrationBuilder.DropChangeTrackingTrigger(
                triggerName: "Users_ChangeTracking");

            migrationBuilder.DropTable(
                name: "PermissionChanges");

            migrationBuilder.DropTable(
                name: "UserChanges");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
