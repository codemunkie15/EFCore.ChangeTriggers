using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Migrations.ChangedByEntity
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangedById = table.Column<int>(type: "int", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUserChanges", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_TestUserChanges_TestUsers_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "TestUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestUserChanges_TestUsers_Id",
                        column: x => x.Id,
                        principalTable: "TestUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.AddNoCheckConstraint(
                table: "TestUserChanges",
                constraint: "FK_TestUserChanges_TestUsers_ChangedById");

            migrationBuilder.AddNoCheckConstraint(
                table: "TestUserChanges",
                constraint: "FK_TestUserChanges_TestUsers_Id");

            migrationBuilder.CreateChangeTrigger(
                trackedTableName: "TestUsers",
                changeTableName: "TestUserChanges",
                triggerName: "TestUsers_ChangeTrigger",
                trackedTablePrimaryKeyColumns: new[] { "Id" },
                changeTableDataColumns: new[] { "DateOfBirth", "Id", "IsAdmin", "LastUpdatedAt", "Username" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"),
                changedByColumn: new ChangeContextColumn("ChangedById", "int"));

            migrationBuilder.InsertData(
                table: "TestUsers",
                columns: new[] { "Id", "DateOfBirth", "IsAdmin", "LastUpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, null, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "f0bffaff-20f1-4366-9b1d-4a00094c6dec" },
                    { 2, null, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Test User 1" },
                    { 3, null, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Test User 2" },
                    { 4, null, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Test User 3" },
                    { 5, null, false, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Test User 4" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestUserChanges_ChangedById",
                table: "TestUserChanges",
                column: "ChangedById");

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
