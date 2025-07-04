using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EFCore.ChangeTriggers.ChangeEventQueries
{
    internal class DbContextAwareEntityQueryProvider : EntityQueryProvider
    {
        public DbContext DbContext { get; init; }

        public DbContextAwareEntityQueryProvider(
            IQueryCompiler queryCompiler,
            ICurrentDbContext currentDbContext)
            : base(queryCompiler)
        {
            DbContext = currentDbContext.Context;
        }
    }
}