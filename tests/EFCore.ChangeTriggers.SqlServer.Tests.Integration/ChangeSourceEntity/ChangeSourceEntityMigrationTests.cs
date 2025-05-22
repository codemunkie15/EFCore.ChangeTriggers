using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Helpers;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Infrastructure;
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

        var testUsers = testHelper.GetTestUsers().ToList();

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
    public void MigrateDatabase_UsingMigrationChangeSource_InsertsChangeEntity_WithCorrectProperties()
    {
        var changeSourceProvider = (ChangeSourceEntityProvider)testHelper.DbContext.GetService<IChangeSourceProvider<ChangeSource>>();
        changeSourceProvider.UseCustomGetMigrationChangeSource = true;

        testHelper.ChangeSourceProvider.MigrationChangeSource = new ChangeSource { Id = 1 };

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.MigrationChangeSource.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 1 };

        await testHelper.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testHelper.GetTestUsers().ToListAsync(TestContext.Current.CancellationToken);

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
    public async Task MigrateDatabase_UsingMigrationChangeSource_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changeSourceProvider = (ChangeSourceEntityProvider)testHelper.DbContext.GetService<IChangeSourceProvider<ChangeSource>>();
        changeSourceProvider.UseCustomGetMigrationChangeSource = true;

        testHelper.ChangeSourceProvider.MigrationChangeSourceAsync = new ChangeSource { Id = 1 };

        await testHelper.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testHelper.GetTestUsers().ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.MigrationChangeSourceAsync.Id);
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

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testHelper.Dispose();
    }
}