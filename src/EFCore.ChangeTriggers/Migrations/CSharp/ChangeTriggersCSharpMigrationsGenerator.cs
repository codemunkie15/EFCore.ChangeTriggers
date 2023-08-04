using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EFCore.ChangeTriggers.Migrations.CSharp
{
    internal class ChangeTriggersCSharpMigrationsGenerator : CSharpMigrationsGenerator
    {
        public ChangeTriggersCSharpMigrationsGenerator(MigrationsCodeGeneratorDependencies dependencies, CSharpMigrationsGeneratorDependencies csharpDependencies)
            : base(dependencies, csharpDependencies)
        {
        }

        protected override IEnumerable<string> GetNamespaces(IEnumerable<MigrationOperation> operations)
        {
            return base.GetNamespaces(operations)
                .Concat(operations.OfType<CreateChangeTriggerOperation>().Select(o => o.GetType().Namespace!))
                .Concat(operations.OfType<DropChangeTriggerOperation>().Select(o => o.GetType().Namespace!));
        }
    }
}
