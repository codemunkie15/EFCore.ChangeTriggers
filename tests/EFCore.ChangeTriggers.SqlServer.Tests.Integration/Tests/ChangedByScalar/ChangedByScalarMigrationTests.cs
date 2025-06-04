using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar;

public class ChangedByScalarMigrationTests : IClassFixture<ChangedByScalarMigrationFixture>, IAsyncLifetime
{
    private readonly ChangedByScalarMigrationFixture fixture;
    private readonly ChangedByScalarTestScope testScope;

    public ChangedByScalarMigrationTests(ChangedByScalarMigrationFixture fixture)
    {
        this.fixture = fixture;

        // Create a new service scope for each test method
        testScope = fixture.CreateTestScope();
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;

        testScope.DbContext.Database.Migrate();

        var testUsers = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUser);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties()
    {
        var changedByProvider = (ChangedByScalarProvider)testScope.DbContext.GetService<IChangedByProvider<string>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testScope.CurrentUserProvider.MigrationCurrentUser = ChangedByScalarUser.SystemUser.Username;

        testScope.DbContext.Database.Migrate();

        var testUsers = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.MigrationCurrentUser);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByScalarUser.SystemUser.Username;

        await testScope.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changedByProvider = (ChangedByScalarProvider)testScope.DbContext.GetService<IChangedByProvider<string>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testScope.CurrentUserProvider.MigrationCurrentUserAsync = ChangedByScalarUser.SystemUser.Username;

        await testScope.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.MigrationCurrentUserAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;

        var migrator = testScope.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangedBy', N'{testScope.CurrentUserProvider.CurrentUser}';");
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        var migrator = testScope.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testScope.Dispose();
    }
}