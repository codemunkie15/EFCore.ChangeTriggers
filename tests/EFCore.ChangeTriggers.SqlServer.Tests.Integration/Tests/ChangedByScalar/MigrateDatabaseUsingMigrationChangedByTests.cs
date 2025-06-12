using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar
{
    public class MigrateDatabaseUsingMigrationChangedByTests :
        MigrateDatabaseTestBase<ChangedByScalarUser, ChangedByScalarUserChange, ChangedByScalarDbContext>,
        IClassFixture<MigrateDatabaseUsingMigrationChangedByFixture>
    {
        private readonly ScalarCurrentUserProvider currentUserProvider;

        public MigrateDatabaseUsingMigrationChangedByTests(MigrateDatabaseUsingMigrationChangedByFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            currentUserProvider.MigrationCurrentUser.Set(useAsync, ChangedByScalarUser.SystemUser.Username);
            return Task.CompletedTask;
        }

        protected override void Assert(ChangedByScalarUserChange userChange, bool useAsync)
        {
            userChange.ChangedBy.Should().Be(currentUserProvider.MigrationCurrentUser.Get(useAsync));
        }
    }
}