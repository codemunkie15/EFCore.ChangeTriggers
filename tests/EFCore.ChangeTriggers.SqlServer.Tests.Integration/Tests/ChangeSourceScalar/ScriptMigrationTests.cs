using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar
{
    public class ScriptMigrationTests : ScriptMigrationTestBase<ChangeSourceScalarDbContext>, IClassFixture<ScriptMigrationFixture>
    {
        private readonly ScalarChangeSourceProvider currentChangeSource;

        public ScriptMigrationTests(ScriptMigrationFixture fixture) : base(fixture.Services)
        {
            currentChangeSource = scope.ServiceProvider.GetRequiredService<ScalarChangeSourceProvider>();
        }

        protected override void SetChangeContext()
        {
            currentChangeSource.CurrentChangeSource.SyncValue = ChangeSourceType.Tests;
        }

        protected override void Assert(IEnumerable<string> scriptLines)
        {
            scriptLines.First().Should().Be(
                $"EXEC sp_set_session_context N'ChangeContext.ChangeSource', {(int)currentChangeSource.CurrentChangeSource.SyncValue};");
        }
    }
}
