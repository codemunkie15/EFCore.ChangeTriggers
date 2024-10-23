using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Migrations.Operations.Generators
{
    internal class ChangedBySetChangeContextOperationGenerator<TChangedBy> : BaseSetChangeContextOperationGenerator, ISetChangeContextOperationGenerator
    {
        private readonly IChangedByProvider<TChangedBy> changedByProvider;

        public ChangedBySetChangeContextOperationGenerator(IChangedByProvider<TChangedBy> changedByProvider)
        {
            this.changedByProvider = changedByProvider;
        }

        public SetChangeContextOperation Generate(IModel model)
        {
            return GenerateSetChangeContextOperation<TChangedBy>(
                model,
                ChangeContextConstants.ChangedByContextName,
                changedByProvider.GetMigrationChangedBy());
        }
    }
}
