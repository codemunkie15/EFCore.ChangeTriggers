using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Metadata;
using EFCore.ChangeTriggers.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Automatically configures any entities in the model that implement <see cref="ITracked{TChangeType}"/> or <see cref="IChange{TTracked}"/>.
        /// </summary>
        /// <param name="modelBuilder">The model builder being used for configuration.</param>
        /// <returns>The model builder so that further configuration can be chained.</returns>
        public static ModelBuilder AutoConfigureChangeTriggers(this ModelBuilder modelBuilder)
        {
            var trackedTypesInModel = modelBuilder.Model.GetEntityTypes()
                .Where(e => e.ClrType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITracked<>)))
                .Select(e => e.ClrType)
                .ToList();

            foreach (var trackedType in trackedTypesInModel)
            {
                var changeType = trackedType.GetInterfaces()
                        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITracked<>))
                        .GetGenericArguments()
                        .First();

                modelBuilder.ConfigureTrackedEntity(trackedType);
                modelBuilder.ConfigureChangeEntity(changeType);
            }

            return modelBuilder;
        }

        private static void ConfigureTrackedEntity(this ModelBuilder modelBuilder, Type trackedType)
        {
            var entityTypeBuilder = modelBuilder.Entity(trackedType);
            entityTypeBuilder.HasChangeTrigger();
        }

        private static void ConfigureChangeEntity(this ModelBuilder modelBuilder, Type changeType)
        {
            var entityTypeBuilder = modelBuilder.Entity(changeType);

            var changeTableBuilder = entityTypeBuilder.IsChangeTable();

            if (changeType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHasChangedBy<>)))
            {
                changeTableBuilder.HasChangedBy();
            }

            if (changeType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHasChangeSource<>)))
            {
                changeTableBuilder.HasChangeSource();
            }
        }
    }
}
