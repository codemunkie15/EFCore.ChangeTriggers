using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Helpers
{
    internal class ChangedByScalarTestHelper : IDisposable
    {
        public ChangedByScalarDbContext DbContext { get; }

        public ScalarCurrentUserProvider CurrentUserProvider { get; }

        private readonly IServiceScope scope;

        public ChangedByScalarTestHelper(IServiceProvider services)
        {
            scope = services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<ChangedByScalarDbContext>();
            CurrentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();
        }

        public ChangedByScalarUser AddTestUser(int id)
        {
            var user = new ChangedByScalarUser(id, $"TestUser{id}");
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IQueryable<ChangedByScalarUser> GetAllTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes);
        }

        public IQueryable<ChangedByScalarUserChange> GetAllTestUserChanges()
        {
            return DbContext.TestUserChanges
                .Include(uc => uc.TrackedEntity);
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}