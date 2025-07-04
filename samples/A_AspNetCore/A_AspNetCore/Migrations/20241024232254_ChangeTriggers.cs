using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_AspNetCore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleChanges",
                columns: table => new
                {
                    ChangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangedById = table.Column<int>(type: "int", nullable: false),
                    ChangeSource = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleChanges", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_RoleChanges_Roles_Id",
                        column: x => x.Id,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleChanges_Users_ChangedById",
                        column: x => x.ChangedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserChanges",
                columns: table => new
                {
                    ChangeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationTypeId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangedById = table.Column<int>(type: "int", nullable: false),
                    ChangeSource = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChanges", x => x.ChangeId);
                    table.ForeignKey(
                        name: "FK_UserChanges_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
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
                name: "IX_RoleChanges_ChangedById",
                table: "RoleChanges",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_RoleChanges_Id",
                table: "RoleChanges",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserChanges_ChangedById",
                table: "UserChanges",
                column: "ChangedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserChanges_Id",
                table: "UserChanges",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserChanges_RoleId",
                table: "UserChanges",
                column: "RoleId");

            migrationBuilder.AddNoCheckConstraint(
                table: "RoleChanges",
                constraint: "FK_RoleChanges_Roles_Id");

            migrationBuilder.AddNoCheckConstraint(
                table: "RoleChanges",
                constraint: "FK_RoleChanges_Users_ChangedById");

            migrationBuilder.AddNoCheckConstraint(
                table: "UserChanges",
                constraint: "FK_UserChanges_Roles_RoleId");

            migrationBuilder.AddNoCheckConstraint(
                table: "UserChanges",
                constraint: "FK_UserChanges_Users_ChangedById");

            migrationBuilder.AddNoCheckConstraint(
                table: "UserChanges",
                constraint: "FK_UserChanges_Users_Id");

            migrationBuilder.CreateChangeTrigger(
                trackedTableName: "Roles",
                changeTableName: "RoleChanges",
                triggerName: "Roles_ChangeTrigger",
                trackedTablePrimaryKeyColumns: new[] { "Id" },
                changeTableDataColumns: new[] { "Enabled", "Id", "Name" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"),
                changeSourceColumn: new ChangeContextColumn("ChangeSource", "int"),
                changedByColumn: new ChangeContextColumn("ChangedById", "int"));

            migrationBuilder.CreateChangeTrigger(
                trackedTableName: "Users",
                changeTableName: "UserChanges",
                triggerName: "Users_ChangeTrigger",
                trackedTablePrimaryKeyColumns: new[] { "Id" },
                changeTableDataColumns: new[] { "DateOfBirth", "Id", "Name", "RoleId" },
                operationTypeColumn: new ChangeContextColumn("OperationTypeId", "int"),
                changedAtColumn: new ChangeContextColumn("ChangedAt"),
                changeSourceColumn: new ChangeContextColumn("ChangeSource", "int"),
                changedByColumn: new ChangeContextColumn("ChangedById", "int"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropChangeTrigger(
                triggerName: "Roles_ChangeTrigger");

            migrationBuilder.DropChangeTrigger(
                triggerName: "Users_ChangeTrigger");

            migrationBuilder.DropTable(
                name: "RoleChanges");

            migrationBuilder.DropTable(
                name: "UserChanges");
        }
    }
}
