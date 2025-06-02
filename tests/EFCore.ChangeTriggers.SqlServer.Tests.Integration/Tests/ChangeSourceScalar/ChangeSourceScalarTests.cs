using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceScalar;

public class ChangeSourceScalarTests : IClassFixture<ChangeSourceScalarFixture>
{
    private readonly ChangeSourceScalarFixture fixture;
    private readonly ChangeSourceScalarTestHelper testHelper;

    public ChangeSourceScalarTests(ChangeSourceScalarFixture fixture)
    {
        this.fixture = fixture;
        testHelper = new ChangeSourceScalarTestHelper(fixture.Services);
    }

    [Fact]
    public void AddEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

        var user = testHelper.AddTestUser();
        testHelper.DbContext.SaveChanges();

        var userChanges = testHelper.GetTestUserChanges().Where(uc => uc.Id == user.Id).ToList();

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task AddEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSourceType.Tests;

        var user = testHelper.AddTestUser();
        await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        var userChanges = await testHelper.GetTestUserChanges()
            .Where(uc => uc.Id == user.Id)
            .ToListAsync(TestContext.Current.CancellationToken);

        var userChange = userChanges.Should().ContainSingle().Which;
        userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        userChange.OperationType.Should().Be(OperationType.Insert);
        userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void AddEntity_WithMultipleScopes_InsertsChangeEntity_WithCorrectProperties()
    {
        var userLookup = new Dictionary<int, ChangeSourceType>();

        foreach (var changeSource in Enum.GetValues<ChangeSourceType>())
        {
            using var testHelperScoped = new ChangeSourceScalarTestHelper(fixture.Services);
            testHelperScoped.ChangeSourceProvider.CurrentChangeSource = changeSource;

            var user = testHelperScoped.AddTestUser();
            testHelperScoped.DbContext.SaveChanges();

            userLookup.Add(user.Id, changeSource);
        }

        var usersFromDb = testHelper.GetTestUsers().Where(u => userLookup.Keys.Contains(u.Id)).ToList();

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
            using var testHelperScoped = new ChangeSourceScalarTestHelper(fixture.Services);
            testHelperScoped.ChangeSourceProvider.CurrentChangeSourceAsync = changeSource;

            var user = testHelperScoped.AddTestUser();
            await testHelperScoped.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            userLookup.Add(user.Id, changeSource);
        }

        var usersFromDb = await testHelper.GetTestUsers().Where(u => userLookup.Keys.Contains(u.Id)).ToListAsync(TestContext.Current.CancellationToken);

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
        testHelper.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

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
        userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task UpdateEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSourceType.Tests;

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
        userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync);
        userChange.TrackedEntity.Id.Should().Be(user.Id);
    }

    [Fact]
    public void DeleteEntity_InsertsChangeEntity_WithCorrectProperties()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSource = ChangeSourceType.Tests;

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
        userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSource);
        userChange.TrackedEntity.Should().BeNull();
    }

    [Fact]
    public async Task DeleteEntity_InsertsChangeEntity_WithCorrectProperties_Async()
    {
        testHelper.ChangeSourceProvider.CurrentChangeSourceAsync = ChangeSourceType.Tests;

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
        userChange.ChangeSource.Should().Be(testHelper.ChangeSourceProvider.CurrentChangeSourceAsync);
        userChange.TrackedEntity.Should().BeNull();
    }
}