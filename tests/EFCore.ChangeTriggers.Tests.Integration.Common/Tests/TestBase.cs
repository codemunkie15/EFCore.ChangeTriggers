using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Tests
{
    public abstract class TestBase<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        protected IServiceScope scope;
        protected TDbContext dbContext;

        protected TestBase(IServiceProvider services)
        {
            UseServiceProvider(services);
        }

        protected virtual void SetupServices(IServiceProvider services) { }

        protected void UseServiceProvider(IServiceProvider services)
        {
            Dispose();

            scope = services.CreateScope();
            dbContext = scope.ServiceProvider.GetService<TDbContext>();
            SetupServices(scope.ServiceProvider);
        }

        public void Dispose()
        {
            scope?.Dispose();
        }
    }
}