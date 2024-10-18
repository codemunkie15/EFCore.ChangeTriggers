using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity
{
    internal class ChangeSourceEntityTestHelper : IDisposable
    {
        public ChangeSourceEntityDbContext DbContext { get; }

        public EntityChangeSourceProvider ChangeSourceProvider { get; }

        private readonly IServiceScope scope;

        public ChangeSourceEntityTestHelper(ChangeSourceEntityFixture fixture)
        {
            scope = fixture.Services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<ChangeSourceEntityDbContext>();
            ChangeSourceProvider = scope.ServiceProvider.GetRequiredService<EntityChangeSourceProvider>();
        }

        public ChangeSourceEntityUser AddTestUser(int id)
        {
            var user = new ChangeSourceEntityUser(id, $"TestUser{id}");
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IQueryable<ChangeSourceEntityUser> GetAllTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes)
                .ThenInclude(c => c.ChangeSource);
        }

        public IQueryable<ChangeSourceEntityUserChange> GetAllTestUserChanges()
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