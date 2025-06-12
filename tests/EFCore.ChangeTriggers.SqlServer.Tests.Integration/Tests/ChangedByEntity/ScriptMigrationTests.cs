using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity
{
    public class ScriptMigrationTests : ScriptMigrationTestBase<ChangedByEntityDbContext>, IClassFixture<ScriptMigrationFixture>
    {
        private readonly EntityCurrentUserProvider currentUserProvider;

        public ScriptMigrationTests(ScriptMigrationFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
        }

        protected override void SetChangeContext()
        {
            currentUserProvider.CurrentUser.SyncValue = ChangedByEntityUser.SystemUser;
        }

        protected override void Assert(IEnumerable<string> scriptLines)
        {
            scriptLines.First().Should().Be(
                $"EXEC sp_set_session_context N'ChangeContext.ChangedBy', {currentUserProvider.CurrentUser.SyncValue.Id};");
        }
    }
}
