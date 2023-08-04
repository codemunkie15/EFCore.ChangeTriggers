using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EFCore.ChangeTriggers.Migrations.Operations
{
    internal class SetChangeContextOperation : MigrationOperation
    {
        public string ContextName { get; set; }

        public object? ContextValue { get; set; }

        public Type ContextValueType { get; set; }
    }
}
