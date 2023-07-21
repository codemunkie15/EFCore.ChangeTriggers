using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    public static class EntityTypeExtensions
    {
        public static bool IsChangeTracked(this IEntityType entityType)
        {
            return entityType.FindAnnotation(AnnotationConstants.UseChangeTrackingTriggers) != null;
        }

        public static IEntityType GetChangeEntityType(this IEntityType entityType)
        {
            var changeEntityTypeName = entityType.GetAnnotation(AnnotationConstants.ChangeEntityTypeName).Value!.ToString()!;
            return entityType.Model.FindEntityType(changeEntityTypeName)!;
        }

        public static string? GetTriggerNameFormat(this IEntityType entityType)
        {
            var annotation = entityType.FindAnnotation(AnnotationConstants.TriggerNameFormat);
            return annotation?.Value?.ToString();
        }

        public static IProperty GetSinglePrimaryKeyProperty(this IEntityType entityType)
        {
            var primaryKey = entityType.FindPrimaryKey();

            if (primaryKey is null)
            {
                // TODO: throw
            }

            if (primaryKey.Properties.Count > 1)
            {
                // TODO: throw
            }

            return primaryKey.Properties.Single();
        }
    }
}