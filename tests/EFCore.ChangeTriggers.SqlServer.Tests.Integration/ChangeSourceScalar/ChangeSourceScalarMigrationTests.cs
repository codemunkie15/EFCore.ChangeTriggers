using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar;

[Collection("SharedContainer")]
public class ChangeSourceScalarMigrationTests : IClassFixture<ChangeSourceScalarMigrationFixture>, IAsyncLifetime
{
    private readonly ChangeSourceScalarMigrationFixture fixture;
    private readonly ChangeSourceScalarTestHelper testHelper;

    public ChangeSourceScalarMigrationTests(ChangeSourceScalarMigrationFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangeSourceScalarTestHelper(fixture.Services);
    }

    [Fact]
    public void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = ChangeSource.Tests;

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetAllTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void MigrateDatabase_UsingMigrationChangeSource_InsertsChangeEntity_WithCorrectProperties()
    {
        var changeSourceProvider = (ChangeSourceScalarProvider)testHelper.DbContext.GetService<IChangeSourceProvider<ChangeSource>>();
        changeSourceProvider.UseCustomGetMigrationChangeSource = true;

        testHelper.ChangeSourceProvider.MigrationChangeSource = ChangeSource.Tests;

        testHelper.DbContext.Database.Migrate();

        var testUsers = testHelper.GetAllTestUsers().ToList();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.MigrationChangeSource);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async void MigrateDatabase_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSource.Tests;

        await testHelper.DbContext.Database.MigrateAsync();

        var testUsers = await testHelper.GetAllTestUsers().ToListAsync();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async void MigrateDatabase_UsingMigrationChangeSource_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changeSourceProvider = (ChangeSourceScalarProvider)testHelper.DbContext.GetService<IChangeSourceProvider<ChangeSource>>();
        changeSourceProvider.UseCustomGetMigrationChangeSource = true;

        testHelper.ChangeSourceProvider.MigrationChangeSourceAsync = ChangeSource.Tests;

        await testHelper.DbContext.Database.MigrateAsync();

        var testUsers = await testHelper.GetAllTestUsers().ToListAsync();

        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.MigrationChangeSourceAsync);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void ScriptMigration_GeneratesSetContextOperation_WithCorrectValuesAndInCorrectPosition()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = ChangeSource.Tests;

        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        var script = migrator.GenerateScript();
        var scriptLines = script.Split(Environment.NewLine);

        scriptLines.FirstOrDefault().Should().Be(
            $"EXEC sp_set_session_context N'ChangeContext.ChangeSource', {(int)testHelper.ChangeSourceProvider.CurrentChangeSource};");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        var migrator = testHelper.DbContext.Database.GetService<IMigrator>();
        await migrator.MigrateAsync("0");

        testHelper.Dispose();
    }
}