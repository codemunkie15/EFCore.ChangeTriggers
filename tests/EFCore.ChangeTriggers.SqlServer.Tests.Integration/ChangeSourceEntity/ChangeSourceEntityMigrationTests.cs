using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity;

public class ChangeSourceEntityMigrationTests : IClassFixture<ChangeSourceEntityMigrationFixture>, IAsyncLifetime
{
    private readonly ChangeSourceEntityMigrationFixture fixture;
    private readonly ChangeSourceEntityTestHelper testHelper;

    public ChangeSourceEntityMigrationTests(ChangeSourceEntityMigrationFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangeSourceEntityTestHelper(fixture.Services);
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = new ChangeSource { Id = 1 };

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetAllTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 1 };

        await testHelper.DbContext.Database.MigrateAsync();

        var testUsers = await testHelper.GetAllTestUsers().ToListAsync();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = new ChangeSource { Id = 1 };

        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangeSource', {(int)testHelper.ChangeSourceProvider.CurrentChangeSource.Id};");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        // Reset current user
        testHelper.ChangeSourceProvider.CurrentChangeSource = null;
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = null;

        testHelper.Dispose();
    }
}