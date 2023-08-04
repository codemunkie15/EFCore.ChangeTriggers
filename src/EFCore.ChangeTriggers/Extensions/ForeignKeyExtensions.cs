using EFCore.ChangeTriggers.Constants;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class ForeignKeyExtensions
    {
        public static bool HasNoCheck(this IForeignKey foreignKey)
        {
            return foreignKey.FindAnnotation(AnnotationConstants.HasNoCheckConstraint) != null;
        }
    }
}
