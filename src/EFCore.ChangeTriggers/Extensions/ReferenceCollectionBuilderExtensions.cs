using EFCore.ChangeTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class ReferenceCollectionBuilderExtensions
    {
        public static ReferenceCollectionBuilder IsChangedByForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsChangedByColumn, true);
        }

        public static ReferenceCollectionBuilder IsChangeSourceForeignKey(this ReferenceCollectionBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.IsChangeSourceColumn, true);
        }

        public static ReferenceCollectionBuilder HasNoCheck(this ReferenceCollectionBuilder builder)
        {
            return builder.HasAnnotation(AnnotationConstants.HasNoCheckConstraint, true);
        }
    }
}
