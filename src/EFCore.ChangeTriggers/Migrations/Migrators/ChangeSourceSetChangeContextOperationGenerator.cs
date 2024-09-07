using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Migrations.Migrators
{
    internal class ChangeSourceSetChangeContextOperationGenerator<TChangeSource> : BaseSetChangeContextOperationGenerator, ISetChangeContextOperationGenerator
    {
        private readonly IChangeSourceProvider<TChangeSource> changeSourceProvider;

        public ChangeSourceSetChangeContextOperationGenerator(IChangeSourceProvider<TChangeSource> changeSourceProvider)
        {
            this.changeSourceProvider = changeSourceProvider;
        }

        public SetChangeContextOperation Generate(IModel model)
        {
            return GenerateSetChangeContextOperation<TChangeSource>(
                model,
                ChangeContextConstants.ChangeSourceContextName,
                changeSourceProvider.GetMigrationChangeSource());
        }
    }
}
