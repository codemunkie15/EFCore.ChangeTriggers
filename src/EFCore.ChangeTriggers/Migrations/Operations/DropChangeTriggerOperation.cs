using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EFCore.ChangeTriggers.Migrations.Operations
{
    public class DropChangeTriggerOperation : MigrationOperation
    {
        public string TriggerName { get; set; }
    }
}
