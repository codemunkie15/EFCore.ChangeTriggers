using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Tests
{
    public abstract class ScriptMigrationTestBase<TDbContext> : TestBase<TDbContext>
        where TDbContext : DbContext
    {
        protected ScriptMigrationTestBase(IServiceProvider services) : base(services)
        {
        }

        protected abstract void SetChangeContext();

        protected abstract void Assert(IEnumerable<string> scriptLines);

        [Fact]
        public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
        {
            SetChangeContext();

            var migrator = dbContext.Database.GetService<IMigrator>();
            var script = migrator.GenerateScript();
            var scriptLines = script.Split(Environment.NewLine);

            scriptLines.Should().NotBeEmpty();
            Assert(scriptLines);
        }
    }
}