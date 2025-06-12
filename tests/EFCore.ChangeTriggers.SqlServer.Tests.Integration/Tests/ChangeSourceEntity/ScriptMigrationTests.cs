using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity
{
    public class ScriptMigrationTests : ScriptMigrationTestBase<ChangeSourceEntityDbContext>, IClassFixture<ScriptMigrationFixture>
    {
        private readonly EntityChangeSourceProvider changeSourceProvider;

        public ScriptMigrationTests(ScriptMigrationFixture fixture) : base(fixture.Services)
        {
            changeSourceProvider = scope.ServiceProvider.GetRequiredService<EntityChangeSourceProvider>();
        }

        protected override void SetChangeContext()
        {
            changeSourceProvider.CurrentChangeSource.SyncValue = new ChangeSource { Id = 1 };
        }

        protected override void Assert(IEnumerable<string> scriptLines)
        {
            scriptLines.First().Should().Be(
                $"EXEC sp_set_session_context N'ChangeContext.ChangeSource', {changeSourceProvider.CurrentChangeSource.SyncValue.Id};");
        }
    }
}
