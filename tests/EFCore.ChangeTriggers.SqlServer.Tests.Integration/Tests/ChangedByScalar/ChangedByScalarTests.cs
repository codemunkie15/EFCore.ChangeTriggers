using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar;

public class ChangedByScalarTests : IClassFixture<ChangedByScalarFixture>
{
    private readonly ChangedByScalarFixture fixture;
    private readonly ChangedByScalarTestScope testScope;

    public ChangedByScalarTests(ChangedByScalarFixture fixture)
    {
        this.fixture = fixture;

        // Create a new service scope for each test method
        testScope = fixture.CreateTestScope();
    }

    [Fact]
    public void AddEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;

        var user = new ChangedByScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        testScope.DbContext.SaveChanges();

        var userChanges = testScope.DbContext.TestUserChanges.Where(uc => uc.Id == user.Id).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUser);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByScalarUser.SystemUser.Username;

        var user = new ChangedByScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Where(uc => uc.Id == user.Id)
            .ToListAsync(cancellationToken: TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        const int numberOfScopes = 50;

        // Create users to use as change context
        testScope.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;
        var changedByUsers = Enumerable.Range(1, numberOfScopes).Select(i => new ChangedByScalarUser()).ToList();
        testScope.DbContext.TestUsers.AddRange(changedByUsers);
        testScope.DbContext.SaveChanges();

        // Dictionary to store user/changedby values to assert later
        var userLookup = new Dictionary<int, string>();

        // Create users in new scopes
        foreach (var changedByUser in changedByUsers)
        {
            using var loopScope = fixture.CreateTestScope();
            loopScope.CurrentUserProvider.CurrentUser = changedByUser.Username;

            var testUser = new ChangedByScalarUser();
            loopScope.DbContext.TestUsers.Add(testUser);
            loopScope.DbContext.SaveChanges();

            userLookup.Add(testUser.Id, changedByUser.Username);
        }

        var testUsersFromDb = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .Where(u => userLookup.Keys.Contains(u.Id))
            .ToList();

        testUsersFromDb.Should().HaveCount(numberOfScopes);
        testUsersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        const int numberOfScopes = 50;

        // Create users to use as change context
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByScalarUser.SystemUser.Username;
        var changedByUsers = Enumerable.Range(1, numberOfScopes).Select(i => new ChangedByScalarUser()).ToList();
        testScope.DbContext.TestUsers.AddRange(changedByUsers);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Dictionary to store user/changedby values to assert later
        var userLookup = new Dictionary<int, string>();

        // Create users in new scopes
        foreach (var changedByUser in changedByUsers)
        {
            using var loopScope = fixture.CreateTestScope();
            loopScope.CurrentUserProvider.CurrentUserAsync = changedByUser.Username;

            var testUser = new ChangedByScalarUser();
            loopScope.DbContext.TestUsers.Add(testUser);
            await loopScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            userLookup.Add(testUser.Id, changedByUser.Username);
        }

        var testUsersFromDb = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .Where(u => userLookup.Keys.Contains(u.Id))
            .ToListAsync(TestContext.Current.CancellationToken);

        testUsersFromDb.Should().HaveCount(numberOfScopes);
        testUsersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangedBy.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void UpdateEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;

        var user = new ChangedByScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        testScope.DbContext.SaveChanges();

        user.Username = "Modified";
        testScope.DbContext.SaveChanges();

        var userChanges = testScope.DbContext.TestUserChanges
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUser);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByScalarUser.SystemUser.Username;

        var user = new ChangedByScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        user.Username = "Modified";
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToListAsync(TestContext.Current.CancellationToken);

        Assert.Single(userChanges);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void DeleteEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.CurrentUserProvider.CurrentUser = ChangedByScalarUser.SystemUser.Username;

        var user = new ChangedByScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        testScope.DbContext.SaveChanges();

        testScope.DbContext.TestUsers.Remove(user);
        testScope.DbContext.SaveChanges();

        var userChanges = testScope.DbContext.TestUserChanges
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUser);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.CurrentUserProvider.CurrentUserAsync = ChangedByScalarUser.SystemUser.Username;

        var user = new ChangedByScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        testScope.DbContext.TestUsers.Remove(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Delete)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Delete);
        userChange.ChangedBy.Should().Be(testScope.CurrentUserProvider.CurrentUserAsync);
        userChange.TrackedEntity.Should().BeNull();
    }
}