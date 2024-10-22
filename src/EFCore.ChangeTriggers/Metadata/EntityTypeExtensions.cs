using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Metadata
{
    internal static class EntityTypeExtensions
    {
        public static bool HasChangeTrigger(this IReadOnlyEntityType entityType)
        {
            return entityType.FindAnnotation(AnnotationConstants.HasChangeTrigger) != null;
        }

        public static bool IsChangeTable(this IReadOnlyEntityType entityType)
        {
            return entityType.FindAnnotation(AnnotationConstants.IsChangeTable) != null;
        }

        public static bool HasChangedBy(this IReadOnlyEntityType entityType)
        {
            return entityType.FindAnnotation(AnnotationConstants.HasChangedBy) != null;
        }

        public static bool HasChangeSource(this IReadOnlyEntityType entityType)
        {
            return entityType.FindAnnotation(AnnotationConstants.HasChangeSource) != null;
        }

        public static IForeignKey GetTrackedEntityForeignKey(this IEntityType entityType)
        {
            return entityType.GetForeignKeys().FirstOrDefault(fk => fk.IsTrackedEntityForeignKey()) ??
                entityType.GetReferencingForeignKeys().FirstOrDefault(fk => fk.IsTrackedEntityForeignKey()) ??
                throw new ChangeTriggersConfigurationException($"The entity {entityType.Name} has no tracked entity foreign key annotation.");
        }

        public static string? GetTriggerNameFormat(this IReadOnlyEntityType entityType)
        {
            var annotation = entityType.FindAnnotation(AnnotationConstants.TriggerNameFormat);
            return annotation?.Value?.ToString();
        }

        public static IProperty GetSinglePrimaryKeyProperty(this IEntityType entityType)
        {
            entityType.EnsureSinglePrimaryKey();

            var primaryKey = entityType.FindPrimaryKey();

            return primaryKey!.Properties.Single();
        }

        public static void EnsureSinglePrimaryKey(this IReadOnlyEntityType entityType)
        {
            var primaryKey = entityType.FindPrimaryKey();

            if (primaryKey is null)
            {
                throw new ChangeTriggersConfigurationException($"{entityType.Name} must have a primary key property to use change triggers.");
            }

            if (primaryKey.Properties.Count > 1)
            {
                throw new ChangeTriggersConfigurationException($"{entityType.Name} must only have one primary key property to use change triggers.");
            }
        }
    }
}