using System;
using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchmarks.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Column1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column13 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column14 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column15 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                    Column1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column9 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column11 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column12 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column13 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column14 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Column15 = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_UserChanges_ChangedById",
                table: "UserChanges",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserChanges_Id",
                table: "UserChanges",
                column: "Id");

            migrationBuilder.AddNoCheckConstraint(
                table: "UserChanges",
                constraint: "FK_UserChanges_Users_ChangedById");

            migrationBuilder.AddNoCheckConstraint(
                table: "UserChanges",
                constraint: "FK_UserChanges_Users_Id");

            migrationBuilder.CreateChangeTrigger(
                trackedTableName: "Users",
                changeTableName: "UserChanges",
                triggerName: "Users_ChangeTrigger",
                trackedTablePrimaryKeyColumns: new[] { "Id" },
                changeTableDataColumns: new[] { "Column1", "Column10", "Column11", "Column12", "Column13", "Column14", "Column15", "Column2", "Column3", "Column4", "Column5", "Column6", "Column7", "Column8", "Column9", "Id" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"),
                changeSourceColumn: new ChangeContextColumn("ChangeSource", "int"),
                changedByColumn: new ChangeContextColumn("ChangedById", "int"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropChangeTrigger(
                triggerName: "Users_ChangeTrigger");

            migrationBuilder.DropTable(
                name: "UserChanges");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
