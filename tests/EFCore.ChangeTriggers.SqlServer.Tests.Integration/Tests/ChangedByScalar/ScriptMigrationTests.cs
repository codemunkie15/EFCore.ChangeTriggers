using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar
{
    public class ScriptMigrationTests : ScriptMigrationTestBase<ChangedByScalarDbContext>, IClassFixture<ScriptMigrationFixture>
    {
        private readonly ScalarCurrentUserProvider currentUserProvider;

        public ScriptMigrationTests(ScriptMigrationFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();
        }

        protected override void SetChangeContext()
        {
            currentUserProvider.CurrentUser.SyncValue = ChangedByScalarUser.SystemUser.Username;
        }

        protected override void Assert(IEnumerable<string> scriptLines)
        {
            scriptLines.First().Should().Be(
                $"EXEC sp_set_session_context N'ChangeContext.ChangedBy', N'{currentUserProvider.CurrentUser.SyncValue}';");
        }
    }
}
