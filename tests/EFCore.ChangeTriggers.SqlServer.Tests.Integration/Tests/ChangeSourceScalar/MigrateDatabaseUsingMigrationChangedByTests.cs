using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar
{
    public class MigrateDatabaseUsingMigrationChangedByTests :
        MigrateDatabaseTestBase<ChangeSourceScalarUser, ChangeSourceScalarUserChange, ChangeSourceScalarDbContext>,
        IClassFixture<MigrateDatabaseUsingMigrationChangedByFixture>
    {
        private readonly ScalarChangeSourceProvider currentChangeSource;

        public MigrateDatabaseUsingMigrationChangedByTests(MigrateDatabaseUsingMigrationChangedByFixture fixture) : base(fixture.Services)
        {
            currentChangeSource = scope.ServiceProvider.GetRequiredService<ScalarChangeSourceProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            currentChangeSource.MigrationChangeSource.Set(useAsync, ChangeSourceType.Migrations);
            return Task.CompletedTask;
        }

        protected override void Assert(ChangeSourceScalarUserChange userChange, bool useAsync)
        {
            userChange.ChangeSource.Should().Be(currentChangeSource.MigrationChangeSource.Get(useAsync));
        }
    }
}