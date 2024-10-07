using EFCore.ChangeTriggers.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCore.ChangeTriggers.Extensions
{
    public static class ModelBuilderExtensions
    {
        private static readonly MethodInfo ModelBuilderEntityMethod = typeof(ModelBuilder).GetMethods()
            .Single(mi => mi.Name.Equals("Entity") && mi.ContainsGenericParameters && !mi.GetParameters().Any());

        private static readonly MethodInfo HasChangeTriggerMethod = typeof(EntityTypeBuilderExtensions)
            .GetMethod(nameof(EntityTypeBuilderExtensions.HasChangeTrigger))!;

        private static readonly IEnumerable<MethodInfo> IsChangeTableMethods = typeof(EntityTypeBuilderExtensions).GetMethods()
            .Where(mi => mi.Name.Equals(nameof(EntityTypeBuilderExtensions.IsChangeTable)));

        /// <summary>
        /// Automatically configures any entities in the model that implement <see cref="ITracked{TChangeType}"/> or <see cref="IChange{TTracked, TChangeId}"/>.
        /// </summary>
        /// <remarks>This method uses reflection.</remarks>
        /// <param name="modelBuilder">The model builder being used for configuration.</param>
        /// <returns>The model builder so that further configuration can be chained.</returns>
        /// <exception cref="ChangeTriggersAutoConfigurationException"></exception>
        public static ModelBuilder AutoConfigureChangeTriggers(this ModelBuilder modelBuilder)
        {
            // TODO: Rewrite this as a source generator?

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

                modelBuilder.ConfigureTrackedEntity(trackedType, changeType);
                modelBuilder.ConfigureChangeEntity(changeType);
            }

            return modelBuilder;
        }

        private static void ConfigureTrackedEntity(this ModelBuilder modelBuilder, Type trackedType, Type changeType)
        {
            dynamic trackedEntityTypeBuilder = CreateEntityTypeBuilder(modelBuilder, trackedType);

            var hasChangeTriggerGenericMethod = HasChangeTriggerMethod.MakeGenericMethod(trackedType, changeType);
            hasChangeTriggerGenericMethod.Invoke(null, new[] { trackedEntityTypeBuilder, null });
        }

        private static void ConfigureChangeEntity(this ModelBuilder modelBuilder, Type changeType)
        {
            dynamic changeEntityTypeBuilder = CreateEntityTypeBuilder(modelBuilder, changeType);

            var typeArguments = new List<Type> { changeType };
            var parameters = new List<object?> { changeEntityTypeBuilder };

            var hasChangedByInterfaceType = changeType
                .GetInterfaces()
                .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHasChangedBy<>));
            if (hasChangedByInterfaceType != null)
            {
                typeArguments.Add(hasChangedByInterfaceType.GetGenericArguments()[0]);
            }

            var hasChangeSourceInterfaceType = changeType
                .GetInterfaces()
                .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHasChangeSource<>));
            if (hasChangeSourceInterfaceType != null)
            {
                typeArguments.Add(hasChangeSourceInterfaceType.GetGenericArguments()[0]);
                parameters.Add(null); // ChangeTableOptions
            }

            var isChangeTableMethod = IsChangeTableMethods
                .Single(mi => mi.GetGenericArguments().Count() == typeArguments.Count && mi.GetParameters().Count() == parameters.Count);

            var isChangeTableGenericMethod = isChangeTableMethod.MakeGenericMethod(typeArguments.ToArray());
            isChangeTableGenericMethod.Invoke(null, parameters.ToArray());
        }

        private static dynamic CreateEntityTypeBuilder(ModelBuilder modelBuilder, Type entityType)
        {
            var modelBuilderEntityGenericMethod = ModelBuilderEntityMethod.MakeGenericMethod(entityType);
            return modelBuilderEntityGenericMethod.Invoke(modelBuilder, null)!;
        }
    }
}
