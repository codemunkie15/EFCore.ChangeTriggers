using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations
{
    public class DropChangeTrackingTriggerOperation : MigrationOperation
    {
        public string TriggerName { get; set; }
    }
}
