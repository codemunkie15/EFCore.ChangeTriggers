using EFCore.ChangeTriggers.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Migrations.Operations.Generators
{
    internal abstract class BaseSetChangeContextOperationGenerator
    {
        protected SetChangeContextOperation GenerateSetChangeContextOperation<TContextValue>(IModel model, string name, object? value)
        {
            return new SetChangeContextOperation
            {
                ContextName = name,
                ContextValue = model.GetRawValue(value)
            };
        }
    }
}