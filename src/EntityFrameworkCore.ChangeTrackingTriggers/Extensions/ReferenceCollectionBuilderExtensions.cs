using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    internal static class ReferenceCollectionBuilderExtensions
    {
        public static ReferenceCollectionBuilder IsChangeContextForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.IsChangeContextColumn, true);
        }

        public static ReferenceCollectionBuilder IsChangedByForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.IsChangedByColumn, true)
                .IsChangeContextForeignKey();
        }

        public static ReferenceCollectionBuilder IsChangeSourceForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.IsChangeSourceColumn, true)
                .IsChangeContextForeignKey();
        }

        public static ReferenceCollectionBuilder HasNoCheck(this ReferenceCollectionBuilder builder)
        {
            return builder
                .HasAnnotation(AnnotationConstants.HasNoCheckConstraint, true);
        }
    }
}
