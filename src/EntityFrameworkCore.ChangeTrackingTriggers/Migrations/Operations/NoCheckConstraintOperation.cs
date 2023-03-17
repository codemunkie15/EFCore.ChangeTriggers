using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations
{
    public class NoCheckConstraintOperation : MigrationOperation, ITableMigrationOperation
    {
        public string? Schema { get; set; }

        public string Table { get; set; }

        public string Constraint { get; set; }
    }
}
