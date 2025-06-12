using EFCore.ChangeTriggers.Tests.Integration.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Scopes
{
    public class TestScope<TContext, TUser, TUserChange> : IDisposable
    {
        public TContext DbContext { get; }

        public IUserReadRepository<TUser, TUserChange> UserReadRepository { get; }

        protected IServiceScope scope;
        private bool disposedValue;

        public TestScope(IServiceProvider services)
        {
            scope = services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            UserReadRepository = scope.ServiceProvider.GetRequiredService<IUserReadRepository<TUser, TUserChange>>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    scope.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
