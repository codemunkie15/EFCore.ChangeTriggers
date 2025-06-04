using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity;

public class ChangedByEntityMigrationTests : IClassFixture<ChangedByEntityMigrationFixture>, IAsyncLifetime
{
    private readonly ChangedByEntityMigrationFixture fixture;
    private readonly ChangedByEntityTestScope testScope;

    public ChangedByEntityMigrationTests(ChangedByEntityMigrationFixture fixture)
    {
        this.fixture = fixture;

        // Create a new service scope for each test method
        testScope = fixture.CreateTestScope();
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        testScope.DbContext.Database.Migrate();

        var testUsers = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUser.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties()
    {
        var changedByProvider = (ChangedByEntityProvider)testScope.DbContext.GetService<IChangedByProvider<ChangedByEntityUser>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testScope.CurrentUserProvider.MigrationCurrentUser = ChangedByEntityUser.SystemUser;

        testScope.DbContext.Database.Migrate();

        var testUsers = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.MigrationCurrentUser.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

        await testScope.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changedByProvider = (ChangedByEntityProvider)testScope.DbContext.GetService<IChangedByProvider<ChangedByEntityUser>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testScope.CurrentUserProvider.MigrationCurrentUserAsync = ChangedByEntityUser.SystemUser;

        await testScope.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Id.Should().Be(testScope.CurrentUserProvider.MigrationCurrentUserAsync.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByEntityUser.SystemUser;

        var migrator = testScope.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangedBy', {testScope.CurrentUserProvider.CurrentUser.Id};");
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        var migrator = testScope.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testScope.Dispose();
    }
}