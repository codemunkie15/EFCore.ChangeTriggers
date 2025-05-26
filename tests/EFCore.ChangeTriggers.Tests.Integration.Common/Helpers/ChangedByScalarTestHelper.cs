using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Helpers
{
    public class ChangedByScalarTestHelper : IDisposable
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

        public ChangedByScalarUser AddTestUser()
        {
            var user = new ChangedByScalarUser();
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IEnumerable<ChangedByScalarUser> AddTestUsers(int count)
        {
            var users = Enumerable.Range(1, count).Select(i => new ChangedByScalarUser()).ToList();
            DbContext.TestUsers.AddRange(users);
            return users;
        }

        public IQueryable<ChangedByScalarUser> GetTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes);
        }

        public IQueryable<ChangedByScalarUserChange> GetTestUserChanges()
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