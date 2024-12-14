using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.Infrastructure;
using System.Reflection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class ChangeTriggersDbContextOptionsBuilderExtensions
    {
        public static void UseChangeEventQueries<TBuilder, TExtension>(
            this ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension> builder,
            Assembly configurationsAssembly)
        where TBuilder : ChangeTriggersDbContextOptionsBuilder<TBuilder, TExtension>
        where TExtension : ChangeTriggersDbContextOptionsExtension, new()
        {
            var extension = builder.OptionsBuilder.GetOrCreateExtension<ChangeEventsDbContextOptionsExtension>()
                .WithConfigurationsAssembly(configurationsAssembly);

            builder.OptionsBuilder.AsInfrastructure().AddOrUpdateExtension(extension);
        }
    }
}