using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Helpers
{
    public class ChangeSourceScalarTestHelper : IDisposable
    {
        public ChangeSourceScalarDbContext DbContext { get; }

        public ScalarChangeSourceProvider ChangeSourceProvider { get; }

        private readonly IServiceScope scope;

        public ChangeSourceScalarTestHelper(IServiceProvider services)
        {
            scope = services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<ChangeSourceScalarDbContext>();
            ChangeSourceProvider = scope.ServiceProvider.GetRequiredService<ScalarChangeSourceProvider>();
        }

        public ChangeSourceScalarUser AddTestUser()
        {
            var user = new ChangeSourceScalarUser();
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IEnumerable<ChangeSourceScalarUser> AddTestUsers(int count)
        {
            var users = Enumerable.Range(1, count).Select(i => new ChangeSourceScalarUser()).ToList();
            DbContext.TestUsers.AddRange(users);
            return users;
        }

        public IQueryable<ChangeSourceScalarUser> GetTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes);
        }

        public IQueryable<ChangeSourceScalarUserChange> GetTestUserChanges()
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