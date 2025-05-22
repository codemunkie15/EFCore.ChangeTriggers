using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Helpers
{
    public class ChangedByEntityTestHelper : IDisposable
    {
        public ChangedByEntityDbContext DbContext { get; }

        public EntityCurrentUserProvider CurrentUserProvider { get; }

        private readonly IServiceScope scope;

        public ChangedByEntityTestHelper(IServiceProvider services)
        {
            scope = services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<ChangedByEntityDbContext>();
            CurrentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
        }

        public ChangedByEntityUser AddTestUser()
        {
            var user = new ChangedByEntityUser();
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IEnumerable<ChangedByEntityUser> AddTestUsers(int count)
        {
            var users = Enumerable.Range(1, count).Select(i => new ChangedByEntityUser()).ToList();
            DbContext.TestUsers.AddRange(users);
            return users;
        }

        public IQueryable<ChangedByEntityUser> GetTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes)
                .ThenInclude(c => c.ChangedBy);
        }

        public IQueryable<ChangedByEntityUserChange> GetTestUserChanges()
        {
            return DbContext.TestUserChanges
                .Include(uc => uc.TrackedEntity)
                .Include(uc => uc.ChangedBy);
        }

        public void Dispose()
        {
            scope.Dispose();
        }
    }
}