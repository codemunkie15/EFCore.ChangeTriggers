using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Extensions
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
            return entityType.FindAnnotation(AnnotationConstants.ChangedByClrTypeName) != null;
        }

        public static bool HasChangeSource(this IReadOnlyEntityType entityType)
        {
            return entityType.FindAnnotation(AnnotationConstants.ChangeSourceClrTypeName) != null;
        }

        public static IEntityType GetTrackedEntityType(this IEntityType entityType)
        {
            var trackedEntityTypeName = entityType.GetAnnotation(AnnotationConstants.TrackedEntityTypeName).Value!.ToString()!;
            return entityType.Model.FindEntityType(trackedEntityTypeName)!;
        }

        public static IEntityType GetChangeEntityType(this IEntityType entityType)
        {
            var changeEntityTypeName = entityType.GetAnnotation(AnnotationConstants.ChangeEntityTypeName).Value!.ToString()!;
            return entityType.Model.FindEntityType(changeEntityTypeName)!;
        }

        public static string GetChangedByClrTypeName(this IReadOnlyEntityType entityType)
        {
            return entityType.GetAnnotation(AnnotationConstants.ChangedByClrTypeName).Value!.ToString()!;
        }

        public static string GetChangeSourceClrTypeName(this IReadOnlyEntityType entityType)
        {
            return entityType.GetAnnotation(AnnotationConstants.ChangeSourceClrTypeName).Value!.ToString()!;
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