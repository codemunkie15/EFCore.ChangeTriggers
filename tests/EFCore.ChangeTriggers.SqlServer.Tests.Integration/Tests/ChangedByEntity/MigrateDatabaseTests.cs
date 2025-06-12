using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity
{
    public class MigrateDatabaseTests :
        MigrateDatabaseTestBase<ChangedByEntityUser, ChangedByEntityUserChange, ChangedByEntityDbContext>,
        IClassFixture<MigrateDatabaseFixture>
    {
        private readonly EntityCurrentUserProvider currentUserProvider;

        public MigrateDatabaseTests(MigrateDatabaseFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            currentUserProvider.CurrentUser.Set(useAsync, ChangedByEntityUser.SystemUser);
            return Task.CompletedTask;
        }

        protected override void Assert(ChangedByEntityUserChange userChange, bool useAsync)
        {
            userChange.ChangedBy.Id.Should().Be(currentUserProvider.CurrentUser.Get(useAsync).Id);
        }
    }
}