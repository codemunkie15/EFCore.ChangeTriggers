using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    internal static class ForeignKeyExtensions
    {
        public static bool HasNoCheck(this IForeignKey foreignKey)
        {
            return foreignKey.FindAnnotation(AnnotationConstants.HasNoCheckConstraint) != null;
        }
    }
}
