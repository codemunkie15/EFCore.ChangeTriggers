using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Fixtures;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Helpers;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity;

[Collection("MsSqlContainer")]
public class ChangeSourceEntityTests : IClassFixture<ChangeSourceEntityFixture>, IAsyncLifetime
{
    private readonly ChangeSourceEntityFixture fixture;
    private readonly ChangeSourceEntityTestHelper testHelper;

    public ChangeSourceEntityTests(ChangeSourceEntityFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangeSourceEntityTestHelper(fixture.Services);
    }

    [Fact]
    public void AddEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser(1);
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async void AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser(1);
        await testHelper.DbContext.SaveChangesAsync();

        var userChanges = await testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1).ToListAsync();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        const int numberOfScopes = 10;

        for (int i = 1; i <= numberOfScopes; i++)
        {
            using var testHelperScoped = new ChangeSourceEntityTestHelper(fixture.Services);
            testHelperScoped.ChangeSourceProvider.CurrentChangeSource = new ChangeSource { Id = i };

            testHelperScoped.AddTestUser(i);
            testHelperScoped.DbContext.SaveChanges();
        }

        var testUsers = testHelper.GetAllTestUsers().Where(u => u.Id >= 1 && u.Id <= numberOfScopes).ToList();

        testUsers.Should().HaveCount(numberOfScopes);
        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(u.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        const int numberOfScopes = 10;

        for (int i = 1; i <= numberOfScopes; i++)
        {
            using var testHelperScoped = new ChangeSourceEntityTestHelper(fixture.Services);
            testHelperScoped.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = i };

            testHelperScoped.AddTestUser(i);
            await testHelperScoped.DbContext.SaveChangesAsync();
        }

        var testUsers = await testHelper.GetAllTestUsers().Where(u => u.Id >= 1 && u.Id <= numberOfScopes).ToListAsync();

        testUsers.Should().HaveCount(numberOfScopes);
        testUsers.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(u.Id);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void UpdateEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser(1);
        testHelper.DbContext.SaveChanges();

        user.Username = "Modified";
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Update).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async void UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser(1);
        await testHelper.DbContext.SaveChangesAsync();

        user.Username = "Modified";
        await testHelper.DbContext.SaveChangesAsync();

        var userChanges = await testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Update).ToListAsync();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void DeleteEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser(1);
        testHelper.DbContext.SaveChanges();

        testHelper.DbContext.TestUsers.Remove(user);
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Delete).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource.Id);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async void DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser(1);
        await testHelper.DbContext.SaveChangesAsync();

        testHelper.DbContext.TestUsers.Remove(user);
        await testHelper.DbContext.SaveChangesAsync();

        var userChanges = await testHelper.GetAllTestUserChanges().Where(uc => uc.Id == 1 && uc.OperationType == OperationType.Delete).ToListAsync();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync.Id);
        userChange.TrackedEntity.Should().BeNull();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 2 };

        // Clear database
        await testHelper.DbContext.TestUsers.ExecuteDeleteAsync();
        await testHelper.DbContext.TestUserChanges.ExecuteDeleteAsync();

        testHelper.Dispose();
    }
}