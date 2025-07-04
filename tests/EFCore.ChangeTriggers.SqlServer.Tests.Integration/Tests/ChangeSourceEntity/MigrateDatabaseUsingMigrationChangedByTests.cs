using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity
{
    public class MigrateDatabaseUsingMigrationChangedByTests :
        MigrateDatabaseTestBase<ChangeSourceEntityUser, ChangeSourceEntityUserChange, ChangeSourceEntityDbContext>,
        IClassFixture<MigrateDatabaseUsingMigrationChangedByFixture>
    {
        private readonly EntityChangeSourceProvider changeSourceProvider;

        public MigrateDatabaseUsingMigrationChangedByTests(MigrateDatabaseUsingMigrationChangedByFixture fixture) : base(fixture.Services)
        {
            changeSourceProvider = scope.ServiceProvider.GetRequiredService<EntityChangeSourceProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            changeSourceProvider.MigrationChangeSource.Set(useAsync, new ChangeSource { Id = 1 });
            return Task.CompletedTask;
        }

        protected override void Assert(ChangeSourceEntityUserChange userChange, bool useAsync)
        {
            userChange.ChangeSource.Id.Should().Be(changeSourceProvider.MigrationChangeSource.Get(useAsync).Id);
        }
    }
}