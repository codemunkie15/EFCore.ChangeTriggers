using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    internal static class PropertyBuilderExtensions
    {
        public static PropertyBuilder IsChangeContextProperty(this PropertyBuilder builder)
        {
            builder.HasAnnotation(AnnotationConstants.IsChangeContextColumn, true);
            return builder;
        }

        public static PropertyBuilder IsOperationTypeProperty(this PropertyBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.IsOperationTypeColumn, true)
                .IsChangeContextProperty();
        }

        public static PropertyBuilder IsChangeSourceProperty(this PropertyBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.IsChangeSourceColumn, true)
                .IsChangeContextProperty();
        }

        public static PropertyBuilder IsChangedAtProperty(this PropertyBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.IsChangedAtColumn, true)
                .IsChangeContextProperty();
        }

        public static PropertyBuilder IsChangedByProperty(this PropertyBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.IsChangedByColumn, true)
                .IsChangeContextProperty();
        }
    }
}
