using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity;

public class ChangeSourceEntityTests : IClassFixture<ChangeSourceEntityFixture>
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

        var user = testHelper.AddTestUser();
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetTestUserChanges().Where(uc => uc.Id == user.Id).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser();
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testHelper.GetTestUserChanges().Where(uc => uc.Id == user.Id).ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        var changeSources = testHelper.DbContext.ChangeSources.ToList();

        var userLookup = new Dictionary<int, int>();

        foreach (var changeSource in changeSources)
        {
            using var testHelperScoped = new ChangeSourceEntityTestHelper(fixture.Services);
            testHelperScoped.ChangeSourceProvider.CurrentChangeSource = changeSource;

            var user = testHelperScoped.AddTestUser();
            testHelperScoped.DbContext.SaveChanges();

            userLookup.Add(user.Id, changeSource.Id);
        }

        var usersFromDb = testHelper.GetTestUsers().Where(u => userLookup.Keys.Contains(u.Id)).ToList();

        usersFromDb.Should().HaveCount(10);
        usersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var changeSources = await testHelper.DbContext.ChangeSources.ToListAsync(TestContext.Current.CancellationToken);

        var userLookup = new Dictionary<int, int>();

        foreach (var changeSource in changeSources)
        {
            using var testHelperScoped = new ChangeSourceEntityTestHelper(fixture.Services);
            testHelperScoped.ChangeSourceProvider.CurrentChangeSourceAsync = changeSource;

            var user = testHelperScoped.AddTestUser();
            await testHelperScoped.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            userLookup.Add(user.Id, changeSource.Id);
        }

        var usersFromDb = await testHelper.GetTestUsers().Where(u => userLookup.Keys.Contains(u.Id)).ToListAsync(TestContext.Current.CancellationToken);

        usersFromDb.Should().HaveCount(10);
        usersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Id.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void UpdateEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser();
        testHelper.DbContext.SaveChanges();

        user.Username = "Modified";
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource.Id);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser();
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        user.Username = "Modified";
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToListAsync(TestContext.Current.CancellationToken);

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

        var user = testHelper.AddTestUser();
        testHelper.DbContext.SaveChanges();

        testHelper.DbContext.TestUsers.Remove(user);
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource.Id);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = new ChangeSource { Id = 2 };

        var user = testHelper.AddTestUser();
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        testHelper.DbContext.TestUsers.Remove(user);
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangeSource.Id.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync.Id);
        userChange.TrackedEntity.Should().BeNull();
    }
}