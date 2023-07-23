using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations
{
    internal class SetChangeTrackingContextOperation : MigrationOperation
    {
        public string ContextName { get; set; }

        public object? ContextValue { get; set; }

        public Type ContextValueType { get; set; }
    }
}
