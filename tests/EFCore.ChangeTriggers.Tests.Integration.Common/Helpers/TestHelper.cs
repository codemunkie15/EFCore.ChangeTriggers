using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Helpers
{
    public class TestHelper : IDisposable
    {
        public TestDbContext DbContext { get; }

        private readonly IServiceScope scope;

        public TestHelper(IServiceProvider services)
        {
            scope = services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        }

        public User AddTestUser()
        {
            var user = new User();
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IEnumerable<User> AddTestUsers(int count)
        {
            var users = Enumerable.Range(1, count).Select(i => new User()).ToList();
            DbContext.TestUsers.AddRange(users);
            return users;
        }

        public IQueryable<User> GetTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes);
        }

        public IQueryable<UserChange> GetTestUserChanges()
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