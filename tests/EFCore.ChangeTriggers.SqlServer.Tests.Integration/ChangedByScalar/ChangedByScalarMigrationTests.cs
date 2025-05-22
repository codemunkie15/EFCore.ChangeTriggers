using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Helpers;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar;

public class ChangedByScalarMigrationTests : IClassFixture<ChangedByScalarMigrationFixture>, IAsyncLifetime
{
    private readonly ChangedByScalarMigrationFixture fixture;
    private readonly ChangedByScalarTestHelper testHelper;

    public ChangedByScalarMigrationTests(ChangedByScalarMigrationFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangedByScalarTestHelper(fixture.Services);
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUser);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties()
    {
        var changedByProvider = (ChangedByScalarProvider)testHelper.DbContext.GetService<IChangedByProvider<string>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testHelper.CurrentUserProvider.MigrationCurrentUser = ChangedByScalarUser.SystemUser.Username;

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.MigrationCurrentUser);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByScalarUser.SystemUser.Username;

        await testHelper.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testHelper.GetTestUsers().ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.CurrentUserAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_UsingMigrationChangedBy_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changedByProvider = (ChangedByScalarProvider)testHelper.DbContext.GetService<IChangedByProvider<string>>();
        changedByProvider.UseCustomGetMigrationChangedBy = true;

        testHelper.CurrentUserProvider.MigrationCurrentUserAsync = ChangedByScalarUser.SystemUser.Username;

        await testHelper.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testHelper.GetTestUsers().ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(testHelper.CurrentUserProvider.MigrationCurrentUserAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
    {
        testHelper.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;

        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangedBy', N'{testHelper.CurrentUserProvider.CurrentUser}';");
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testHelper.Dispose();
    }
}