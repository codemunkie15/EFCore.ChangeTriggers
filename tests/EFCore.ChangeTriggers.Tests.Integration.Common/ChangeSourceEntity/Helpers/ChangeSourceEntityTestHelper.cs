using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Helpers
{
    public class ChangeSourceEntityTestHelper : IDisposable
    {
        public ChangeSourceEntityDbContext DbContext { get; }

        public EntityChangeSourceProvider ChangeSourceProvider { get; }

        private readonly IServiceScope scope;

        public ChangeSourceEntityTestHelper(IServiceProvider services)
        {
            scope = services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<ChangeSourceEntityDbContext>();
            ChangeSourceProvider = scope.ServiceProvider.GetRequiredService<EntityChangeSourceProvider>();
        }

        public ChangeSourceEntityUser AddTestUser()
        {
            var user = new ChangeSourceEntityUser();
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IEnumerable<ChangeSourceEntityUser> AddTestUsers(int count)
        {
            var users = Enumerable.Range(1, count).Select(i => new ChangeSourceEntityUser()).ToList();
            DbContext.TestUsers.AddRange(users);
            return users;
        }

        public IQueryable<ChangeSourceEntityUser> GetTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes)
                .ThenInclude(c => c.ChangeSource);
        }

        public IQueryable<ChangeSourceEntityUserChange> GetTestUserChanges()
        {
            return DbContext.TestUserChanges
                .Include(uc => uc.TrackedEntity)
                .Include(uc => uc.ChangeSource);
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}