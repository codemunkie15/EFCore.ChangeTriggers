using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Migrations.Operations.Generators
{
    internal interface ISetChangeContextOperationGenerator
    {
        SetChangeContextOperation Generate(IModel model);
    }
}
