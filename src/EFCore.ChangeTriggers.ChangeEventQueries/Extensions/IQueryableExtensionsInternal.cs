using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Extensions
{
    internal static class IQueryableExtensionsInternal
    {
        public static void EnsureElementType<TElementType>(this IQueryable queryable) where TElementType : class
        {
            // Check if the element type implements the specified interface
            if (!typeof(TElementType).IsAssignableFrom(queryable.ElementType))
            {
                throw new InvalidOperationException(ExceptionStrings.QueryableInvalidElementType(typeof(TElementType).Name));
            }
        }
    }
}