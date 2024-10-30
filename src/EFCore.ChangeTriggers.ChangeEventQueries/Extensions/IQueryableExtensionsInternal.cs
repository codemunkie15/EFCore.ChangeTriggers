using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

        public static DbContext GetDbContext(this IQueryable query)
        {
            if (query is IInfrastructure<IServiceProvider> infrastructure)
            {
                var dbContextServices = infrastructure.Instance.GetRequiredService<IDbContextServices>();
                return dbContextServices.CurrentContext.Context;
            }

            throw new ChangeEventQueryException("The provided query is not an EF Core queryable.");
        }
    }
}