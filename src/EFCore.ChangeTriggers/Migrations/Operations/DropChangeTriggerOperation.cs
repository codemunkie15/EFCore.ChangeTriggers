using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EFCore.ChangeTriggers.Migrations.Operations
{
    public class DropChangeTriggerOperation : MigrationOperation
    {
        public required string TriggerName { get; set; }
    }
}
