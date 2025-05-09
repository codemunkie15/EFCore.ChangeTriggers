using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar
{
    internal class ChangeSourceScalarTestHelper : IDisposable
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

        public ChangeSourceScalarUser AddTestUser(int id)
        {
            var user = new ChangeSourceScalarUser(id, $"TestUser{id}");
            DbContext.TestUsers.Add(user);
            return user;
        }

        public IQueryable<ChangeSourceScalarUser> GetAllTestUsers()
        {
            return DbContext.TestUsers
                .Include(u => u.Changes);
        }

        public IQueryable<ChangeSourceScalarUserChange> GetAllTestUserChanges()
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