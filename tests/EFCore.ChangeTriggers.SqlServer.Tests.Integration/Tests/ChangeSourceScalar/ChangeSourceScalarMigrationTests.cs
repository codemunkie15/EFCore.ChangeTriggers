using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar;

public class ChangeSourceScalarMigrationTests : IClassFixture<ChangeSourceScalarMigrationFixture>, IAsyncLifetime
{
    private readonly ChangeSourceScalarMigrationFixture fixture;
    private readonly ChangeSourceScalarTestScope testScope;

    public ChangeSourceScalarMigrationTests(ChangeSourceScalarMigrationFixture fixture)
    {
        this.fixture = fixture;
        testScope = fixture.CreateTestScope();
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

        testScope.DbContext.Database.Migrate();

        var testUsers = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSource);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void MigrateDatabase_UsingMigrationChangeSource_InsertsChangeEntity_WithCorrectProperties()
    {
        var changeSourceProvider = (ChangeSourceScalarProvider)testScope.DbContext.GetService<IChangeSourceProvider<ChangeSourceType>>();
        changeSourceProvider.UseCustomGetMigrationChangeSource = true;

        testScope.ChangeSourceProvider.MigrationChangeSource = ChangeSourceType.Tests;

        testScope.DbContext.Database.Migrate();

        var testUsers = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.MigrationChangeSource);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSourceType.Tests;

        await testScope.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSourceAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task MigrateDatabase_UsingMigrationChangeSource_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changeSourceProvider = (ChangeSourceScalarProvider)testScope.DbContext.GetService<IChangeSourceProvider<ChangeSourceType>>();
        changeSourceProvider.UseCustomGetMigrationChangeSource = true;

        testScope.ChangeSourceProvider.MigrationChangeSourceAsync = ChangeSourceType.Tests;

        await testScope.DbContext.Database.MigrateAsync(TestContext.Current.CancellationToken);

        var testUsers = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .ToListAsync(TestContext.Current.CancellationToken);

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.MigrationChangeSourceAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
    {
        testScope.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

        var migrator = testScope.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangeSource', {(int)testScope.ChangeSourceProvider.CurrentChangeSource};");
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        var migrator = testScope.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testScope.Dispose();
    }
}