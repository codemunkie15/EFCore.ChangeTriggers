using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity
{
    internal class ChangedByEntityTestHelper : IDisposable
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

        public ChangedByEntityUser AddTestUser(int id)
        {
            var user = new ChangedByEntityUser(id, $"TestUser{id}");
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IQueryable<ChangedByEntityUser> GetAllTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes)
                .ThenInclude(c => c.ChangedBy);
        }

        public IQueryable<ChangedByEntityUserChange> GetAllTestUserChanges()
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