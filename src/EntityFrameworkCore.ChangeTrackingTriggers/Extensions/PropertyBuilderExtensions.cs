using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    internal static class PropertyBuilderExtensions
    {
        public static PropertyBuilder IsOperationTypeProperty(this PropertyBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsOperationTypeColumn, true);
        }

        public static PropertyBuilder IsChangeSourceProperty(this PropertyBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsChangeSourceColumn, true);
        }

        public static PropertyBuilder IsChangedAtProperty(this PropertyBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsChangedAtColumn, true);
        }

        public static PropertyBuilder IsChangedByProperty(this PropertyBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsChangedByColumn, true);
        }
    }
}
