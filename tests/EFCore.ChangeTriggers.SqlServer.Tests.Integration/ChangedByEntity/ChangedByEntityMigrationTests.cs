using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Helpers;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity;

public class ChangedByEntityMigrationTests : IClassFixture<ChangedByEntityMigrationFixture>, IAsyncLifetime
{
    private readonly ChangedByEntityMigrationFixture fixture;
    private readonly ChangedByEntityTestHelper testHelper;

    public ChangedByEntityMigrationTests(ChangedByEntityMigrationFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangedByEntityTestHelper(fixture.Services); // Use a new helper per test method
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetTestUsers().ToList();

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

        testHelper.CurrentUserProvider.MigrationCurrentUser = ChangedByEntityUser.SystemUser;

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetTestUsers().ToList();

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
    public async Task MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        await testHelper.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testHelper.GetTestUsers().ToListAsync(TestContext.Current.CancellationToken);

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
    public async Task MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changedByProvider = (ChangedByEntityProvider)testHelper.DbContext.GetService<IChangedByProvider<ChangedByEntityUser>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testHelper.CurrentUserProvider.MigrationCurrentUserAsync = ChangedByEntityUser.SystemUser;

        await testHelper.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testHelper.GetTestUsers().ToListAsync(TestContext.Current.CancellationToken);

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
        testHelper.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangedBy', {testHelper.CurrentUserProvider.CurrentUser.Id};");
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testHelper.Dispose();
    }
}