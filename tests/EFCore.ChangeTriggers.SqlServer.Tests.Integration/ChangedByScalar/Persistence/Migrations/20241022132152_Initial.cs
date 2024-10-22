using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestUserChanges",
                columns: table => new
                {
                    ChangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUserChanges", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_TestUserChanges_TestUsers_Id",
                        column: x => x.Id,
                        principalTable: "TestUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.AddNoCheckConstraint(
                table: "TestUserChanges",
                constraint: "FK_TestUserChanges_TestUsers_Id");

            migrationBuilder.CreateChangeTrigger(
                trackedTableName: "TestUsers",
                changeTableName: "TestUserChanges",
                triggerName: "TestUsers_ChangeTrigger",
                trackedTablePrimaryKeyColumns: new[] { "Id" },
                changeTableDataColumns: new[] { "Id", "Username" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"),
                changedByColumn: new ChangeContextColumn("ChangedBy", "nvarchar(max)"));

            migrationBuilder.InsertData(
                table: "TestUsers",
                columns: new[] { "Id", "Username" },
                values: new object[,]
                {
                    { 0, "System" },
                    { 100, "TestUser100" },
                    { 101, "TestUser101" },
                    { 102, "TestUser102" },
                    { 103, "TestUser103" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestUserChanges_Id",
                table: "TestUserChanges",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropChangeTrigger(
                triggerName: "TestUsers_ChangeTrigger");

            migrationBuilder.DropTable(
                name: "TestUserChanges");

            migrationBuilder.DropTable(
                name: "TestUsers");
        }
    }
}
