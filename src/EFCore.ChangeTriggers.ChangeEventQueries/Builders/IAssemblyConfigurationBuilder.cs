using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal interface IAssemblyConfigurationBuilder
    {
        ChangeEventConfiguration Build(Assembly configurationsAssembly);
    }
}
