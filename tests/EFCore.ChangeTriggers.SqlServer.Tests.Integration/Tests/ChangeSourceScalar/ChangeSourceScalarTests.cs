using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Scopes;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar;

public class ChangeSourceScalarTests : IClassFixture<ChangeSourceScalarFixture>
{
    private readonly ChangeSourceScalarFixture fixture;
    private readonly ChangeSourceScalarTestScope testScope;

    public ChangeSourceScalarTests(ChangeSourceScalarFixture fixture)
    {
        this.fixture = fixture;
        testScope = fixture.CreateTestScope();
    }

    [Fact]
    public void AddEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

        var user = new ChangeSourceScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        testScope.DbContext.SaveChanges();

        var userChanges = testScope.DbContext.TestUserChanges.Where(uc => uc.Id == user.Id).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSource);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSourceType.Tests;

        var user = new ChangeSourceScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Where(uc => uc.Id == user.Id)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSourceAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        var userLookup = new Dictionary<int, ChangeSourceType>();

        foreach (var changeSource in Enum.GetValues<ChangeSourceType>())
        {
            using var loopScope = fixture.CreateTestScope();
            loopScope.ChangeSourceProvider.CurrentChangeSource = changeSource;

            var user = new ChangeSourceScalarUser();
            loopScope.DbContext.TestUsers.Add(user);
            loopScope.DbContext.SaveChanges();

            userLookup.Add(user.Id, changeSource);
        }

        var usersFromDb = testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .Where(u => userLookup.Keys.Contains(u.Id)).ToList();

        usersFromDb.Should().HaveCount(11);
        usersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public async Task AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        var userLookup = new Dictionary<int, ChangeSourceType>();

        foreach (var changeSource in Enum.GetValues<ChangeSourceType>())
        {
            using var loopScope = fixture.CreateTestScope();
            loopScope.ChangeSourceProvider.CurrentChangeSourceAsync = changeSource;

            var user = new ChangeSourceScalarUser();
            loopScope.DbContext.TestUsers.Add(user);
            await loopScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            userLookup.Add(user.Id, changeSource);
        }

        var usersFromDb = await testScope.DbContext.TestUsers
            .Include(u => u.Changes)
            .Where(u => userLookup.Keys.Contains(u.Id)).ToListAsync(TestContext.Current.CancellationToken);

        usersFromDb.Should().HaveCount(11);
        usersFromDb.Should().AllSatisfy(u =>
        {
            var userChange = u.Changes.Should().ContainSingle().Which;

            userChange.Should().BeEquivalentTo(u, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(OperationType.Insert);
            userChange.ChangeSource.Should().Be(userLookup[u.Id]);
            userChange.TrackedEntity.Id.Should().Be(u.Id);
        });
    }

    [Fact]
    public void UpdateEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

        var user = new ChangeSourceScalarUser();
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
        userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSource);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSourceType.Tests;

        var user = new ChangeSourceScalarUser();
        testScope.DbContext.TestUsers.Add(user);
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        user.Username = "Modified";
        await testScope.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testScope.DbContext.TestUserChanges
            .Where(uc => uc.Id == user.Id && uc.OperationType == OperationType.Update)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Update);
        userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSourceAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void DeleteEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testScope.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

        var user = new ChangeSourceScalarUser();
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
        userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSource);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testScope.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSourceType.Tests;

        var user = new ChangeSourceScalarUser();
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
        userChange.ChangeSource.Should().Be(testScope.ChangeSourceProvider.CurrentChangeSourceAsync);
        userChange.TrackedEntity.Should().BeNull();
    }
}