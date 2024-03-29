﻿using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class EntityTypeExtensions
    {
        public static bool IsChangeTracked(this IReadOnlyEntityType entityType)
        {
            return entityType.FindAnnotation(AnnotationConstants.UseChangeTriggers) != null;
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