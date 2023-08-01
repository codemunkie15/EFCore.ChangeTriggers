﻿using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    public static class ModelBuilderExtensions
    {
        private static readonly MethodInfo ModelBuilderEntityMethod = typeof(ModelBuilder).GetMethods()
            .Single(mi => mi.Name.Equals("Entity") && mi.ContainsGenericParameters && !mi.GetParameters().Any());

        private static readonly MethodInfo HasChangeTrackingTriggerMethod = typeof(EntityTypeBuilderExtensions)
            .GetMethod(nameof(EntityTypeBuilderExtensions.HasChangeTrackingTrigger))!;

        private static readonly IEnumerable<MethodInfo> IsChangeTrackingTableMethods = typeof(EntityTypeBuilderExtensions).GetMethods()
            .Where(mi => mi.Name.Equals(nameof(EntityTypeBuilderExtensions.IsChangeTrackingTable)));

        /// <summary>
        /// Automatically configures any entities in the model that implement <see cref="ITracked{TChangeType}"/> or <see cref="IChange{TTracked, TChangeId}"/>.
        /// </summary>
        /// <remarks>This method uses reflection.</remarks>
        /// <param name="modelBuilder">The model builder being used for configuration.</param>
        /// <returns>The model builder so that further configuration can be chained.</returns>
        /// <exception cref="ChangeTrackingTriggersAutoConfigurationException"></exception>
        public static ModelBuilder AutoConfigureChangeTrackingTriggers(this ModelBuilder modelBuilder)
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

                var changeIdType = GetChangeIdType(trackedType, changeType);

                modelBuilder.ConfigureTrackedEntity(trackedType, changeType);
                modelBuilder.ConfigureChangeEntity(changeType, changeIdType);
            }

            return modelBuilder;
        }

        private static void ConfigureTrackedEntity(this ModelBuilder modelBuilder, Type trackedType, Type changeType)
        {
            dynamic trackedEntityTypeBuilder = CreateEntityTypeBuilder(modelBuilder, trackedType);

            var hasChangeTrackingTriggerGenericMethod = HasChangeTrackingTriggerMethod.MakeGenericMethod(trackedType, changeType);
            hasChangeTrackingTriggerGenericMethod.Invoke(null, new[] { trackedEntityTypeBuilder, null });
        }

        private static void ConfigureChangeEntity(this ModelBuilder modelBuilder, Type changeType, Type changeIdType)
        {
            dynamic changeEntityTypeBuilder = CreateEntityTypeBuilder(modelBuilder, changeType);

            var typeArguments = new List<Type> { changeType, changeIdType };
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
                parameters.Add(null); // ChangeTrackingTableOptions
            }

            var isChangeTrackingTableMethod = IsChangeTrackingTableMethods
                .Single(mi => mi.GetGenericArguments().Count() == typeArguments.Count && mi.GetParameters().Count() == parameters.Count);

            var isChangeTrackingTableGenericMethod = isChangeTrackingTableMethod.MakeGenericMethod(typeArguments.ToArray());
            isChangeTrackingTableGenericMethod.Invoke(null, parameters.ToArray());
        }

        private static dynamic CreateEntityTypeBuilder(ModelBuilder modelBuilder, Type entityType)
        {
            var modelBuilderEntityGenericMethod = ModelBuilderEntityMethod.MakeGenericMethod(entityType);
            return modelBuilderEntityGenericMethod.Invoke(modelBuilder, null)!;
        }

        private static Type GetChangeIdType(Type trackedType, Type changeType)
        {
            var changeInterfaceType = changeType.GetInterfaces()
                    .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IChange<,>));

            if (changeInterfaceType == null)
            {
                throw new ChangeTrackingTriggersConfigurationException(
                    $"The type '{changeType.Name}' needs to implement the IChange interface to be used as the change type for tracked type '{trackedType.Name}'.");
            }

            return changeInterfaceType.GetGenericArguments()[1];
        }
    }
}
