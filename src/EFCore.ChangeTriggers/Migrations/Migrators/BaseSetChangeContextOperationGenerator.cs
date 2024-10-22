using EFCore.ChangeTriggers.Metadata;
using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Migrations.Migrators
{
    internal abstract class BaseSetChangeContextOperationGenerator
    {
        protected SetChangeContextOperation GenerateSetChangeContextOperation<TContextValue>(IModel model, string name, object? value)
        {
            return new SetChangeContextOperation
            {
                ContextName = name,
                ContextValue = model.GetRawValue<TContextValue>(value),
                ContextValueType = model.GetRawValueType(typeof(TContextValue))
            };
        }
    }
}
