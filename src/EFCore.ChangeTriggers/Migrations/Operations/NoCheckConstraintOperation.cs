using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EFCore.ChangeTriggers.Migrations.Operations
{
    public class NoCheckConstraintOperation : MigrationOperation, ITableMigrationOperation
    {
        public string? Schema { get; set; }

        public required string Table { get; set; }

        public required string Constraint { get; set; }
    }
}
