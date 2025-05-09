using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Helpers;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity;

[Collection("ChangedByEntity")]
public class ChangedByEntityMigrationTests : IClassFixture<ChangedByEntityMigrationFixture>, IAsyncLifetime
{
    private readonly ChangedByEntityMigrationFixture fixture;
    private readonly ChangedByEntityTestHelper testHelper;

    public ChangedByEntityMigrationTests(ChangedByEntityMigrationFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangedByEntityTestHelper(fixture.Services);
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = new ChangedByEntityUser { Id = 0 };

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetAllTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUser.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties()
    {
        var changedByProvider = (ChangedByEntityProvider)testHelper.DbContext.GetService<IChangedByProvider<ChangedByEntityUser>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testHelper.CurrentUserProvider.MigrationCurrentUser = new ChangedByEntityUser { Id = 0 };

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetAllTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.MigrationCurrentUser.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = new ChangedByEntityUser { Id = 0 };

        await testHelper.DbContext.Database.MigrateAsync();

        var testUsers = await testHelper.GetAllTestUsers().ToListAsync();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async void MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changedByProvider = (ChangedByEntityProvider)testHelper.DbContext.GetService<IChangedByProvider<ChangedByEntityUser>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testHelper.CurrentUserProvider.MigrationCurrentUserAsync = new ChangedByEntityUser { Id = 0 };

        await testHelper.DbContext.Database.MigrateAsync();

        var testUsers = await testHelper.GetAllTestUsers().ToListAsync();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testHelper.CurrentUserProvider.MigrationCurrentUserAsync.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
    {
        testHelper.CurrentUserProvider.CurrentUser = new ChangedByEntityUser { Id = 0 };

        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangedBy', {testHelper.CurrentUserProvider.CurrentUser.Id};");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testHelper.Dispose();
    }
}