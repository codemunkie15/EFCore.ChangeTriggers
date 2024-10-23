using EFCore.ChangeTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Migrations.Operations.OperationGenerators
{
    internal interface ISetChangeContextOperationGenerator
    {
        SetChangeContextOperation Generate(IModel model);
    }
}
