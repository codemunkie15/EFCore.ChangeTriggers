using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Tests
{
    public abstract class TestBase<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        protected readonly IServiceScope scope;
        protected readonly TDbContext dbContext;

        protected TestBase(IServiceProvider services)
        {
            scope = services.CreateScope();
            dbContext = scope.ServiceProvider.GetService<TDbContext>();
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}