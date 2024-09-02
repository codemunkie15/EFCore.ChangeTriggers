using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EFCore.ChangeTriggers.Migrations.Operations
{
    internal class SetChangeContextOperation : MigrationOperation
    {
        public required string ContextName { get; set; }

        public object? ContextValue { get; set; }

        public required Type ContextValueType { get; set; }
    }
}
