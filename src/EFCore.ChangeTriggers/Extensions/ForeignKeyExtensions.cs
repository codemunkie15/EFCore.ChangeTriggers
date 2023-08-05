using EFCore.ChangeTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class ForeignKeyExtensions
    {
        public static IMutableForeignKey HasNoCheck(this IMutableForeignKey foreignKey)
        {
            foreignKey.AddAnnotation(AnnotationConstants.HasNoCheckConstraint, true);
            return foreignKey;
        }

        public static bool HasNoCheck(this IForeignKey foreignKey)
        {
            return foreignKey.FindAnnotation(AnnotationConstants.HasNoCheckConstraint) != null;
        }
    }
}
