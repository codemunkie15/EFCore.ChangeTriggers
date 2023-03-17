using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.CSharp
{
    internal class ChangeTrackingCSharpMigrationsGenerator : CSharpMigrationsGenerator
    {
        public ChangeTrackingCSharpMigrationsGenerator(MigrationsCodeGeneratorDependencies dependencies, CSharpMigrationsGeneratorDependencies csharpDependencies)
            : base(dependencies, csharpDependencies)
        {
        }

        protected override IEnumerable<string> GetNamespaces(IEnumerable<MigrationOperation> operations)
        {
            return base.GetNamespaces(operations)
                .Concat(operations.OfType<CreateChangeTrackingTriggerOperation>().Select(o => o.GetType().Namespace!))
                .Concat(operations.OfType<DropChangeTrackingTriggerOperation>().Select(o => o.GetType().Namespace!));
        }
    }
}
