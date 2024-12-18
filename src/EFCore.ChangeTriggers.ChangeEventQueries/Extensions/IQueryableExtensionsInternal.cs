using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using Microsoft.EntityFrameworkCore;

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
            var entityQueryProvider = query.Provider as DbContextAwareEntityQueryProvider
                ?? throw new ChangeEventQueryException(
                    "Queryable does not implement the correct provider to use ChangeEventQueries. Have you configured it on your DbContext?");

            return entityQueryProvider.DbContext;
        }

        //public static DbContext GetDbContext(this IQueryable query)
        //{
        //    var compilerField = typeof(EntityQueryProvider).GetField("_queryCompiler", BindingFlags.NonPublic | BindingFlags.Instance)
        //        ?? throw new ChangeEventQueryException("Could not find _queryCompiler private field on query provider.");

        //    var compiler = compilerField.GetValue(query.Provider) as IQueryCompiler
        //        ?? throw new ChangeEventQueryException("QueryCompiler fetched from query provider is null.");

        //    var queryContextFactoryField = compiler.GetType().GetField("_queryContextFactory", BindingFlags.NonPublic | BindingFlags.Instance)
        //        ?? throw new ChangeEventQueryException("Could not find _queryContextFactory private field on query complier.");

        //    var queryContextFactory = queryContextFactoryField.GetValue(compiler) as IQueryContextFactory
        //        ?? throw new ChangeEventQueryException("QueryContextFactory fetched from query compiler is null.");

        //    var queryContext = queryContextFactory.Create();
        //    return queryContext.Context;
        //}
    }
}