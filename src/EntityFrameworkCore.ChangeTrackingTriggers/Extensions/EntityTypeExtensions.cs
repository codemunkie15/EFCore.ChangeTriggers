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
    }
}