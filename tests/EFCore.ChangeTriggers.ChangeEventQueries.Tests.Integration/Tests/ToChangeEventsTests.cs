using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using FluentAssertions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests
{
    public class ToChangeEventsTests : ToChangeEventsTestBase<User, UserChange, TestDbContext, ChangeEvent>, IClassFixture<ToChangeEventsFixture>
    {

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
        }

        protected override IQueryable<ChangeEvent> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration)
        {
            return query.ToChangeEvents(configuration);
        }

        [Fact]
        public void ToChangeEvents_WithNoConfiguration_ThrowsException()
        {
            var changeEvents = () => dbContext.TestUserChanges.ToChangeEvents();

            changeEvents.Should()
                .Throw<ChangeEventQueryException>()
                .WithMessage("No configuration found.*");
        }

        [Fact]
        public void ToChangeEvents_WithNoConfigurationForEntity_ThrowsException()
        {
            var changeEvents = () => dbContext.TestUserChanges.ToChangeEvents(new ChangeEventConfiguration());

            changeEvents.Should()
                .Throw<ChangeEventQueryException>()
                .WithMessage("No configuration found for entity type UserChange.");
        }

        //[Fact]
        //public async Task ToChangeEvents_WithDbContextConfiguration_ReturnsCorrectChangeEvents()
        //{
        //    var oldUser = new User
        //    {
        //        Username = "DbContext User",
        //        IsAdmin = false,
        //        LastUpdatedAt = DateTimeOffset.UtcNow
        //    };
        //    var newUser = new User
        //    {
        //        Username = "DbContext User Changed",
        //        IsAdmin = true,
        //        LastUpdatedAt = DateTimeOffset.UtcNow.AddHours(2)
        //    };
        //    var user = new User
        //    {
        //        Username = oldUser.Username,
        //        IsAdmin = oldUser.IsAdmin,
        //        LastUpdatedAt = oldUser.LastUpdatedAt
        //    };
        //    dbContext.TestUsers.Add(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        //    user.Username = newUser.Username;
        //    user.IsAdmin = newUser.IsAdmin;
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        //    user.LastUpdatedAt = newUser.LastUpdatedAt;
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        //    // Assume the fixture or OnModelCreating auto-registers config for Username, IsAdmin, LastUpdatedAt
        //    var changeEvents = await dbContext.TestUserChanges
        //        .Where(uc => uc.Id == user.Id)
        //        .ToChangeEvents()
        //        .ToListAsync(TestContext.Current.CancellationToken);

        //    changeEvents.Should().NotBeEmpty(); // At least one event should be present
        //}

        //[Fact]
        //public async Task ToChangeEvents_WithInsertAndDeleteEvents_ReturnsInsertAndDeleteEvents()
        //{
        //    var user = new User { Username = "InsertDeleteUser", IsAdmin = false, LastUpdatedAt = DateTimeOffset.UtcNow };
        //    dbContext.TestUsers.Add(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        //    dbContext.TestUsers.Remove(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        //    var config = new ChangeEventConfiguration(builder =>
        //    {
        //        builder.Configure<UserChange>(uc =>
        //        {
        //            uc.AddProperty(uc => uc.Username);
        //            uc.AddInserts();
        //            uc.AddDeletes();
        //        });
        //    });

        //    var changeEvents = await dbContext.TestUserChanges
        //        .Where(uc => uc.Id == user.Id)
        //        .ToChangeEvents(config)
        //        .ToListAsync(TestContext.Current.CancellationToken);

        //    changeEvents.Should().Contain(e => e.Description.Contains("insert", StringComparison.OrdinalIgnoreCase));
        //    changeEvents.Should().Contain(e => e.Description.Contains("delete", StringComparison.OrdinalIgnoreCase));
        //}

        //[Fact]
        //public async Task ToChangeEvents_WithNullPropertyValue_HandlesNullsCorrectly()
        //{
        //    var user = new User { Username = null, IsAdmin = false, LastUpdatedAt = DateTimeOffset.UtcNow };
        //    dbContext.TestUsers.Add(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        //    user.Username = "NotNull";
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        //    var config = new ChangeEventConfiguration(builder =>
        //    {
        //        builder.Configure<UserChange>(uc =>
        //        {
        //            uc.AddProperty(uc => uc.Username);
        //        });
        //    });

        //    var changeEvents = await dbContext.TestUserChanges
        //        .Where(uc => uc.Id == user.Id)
        //        .ToChangeEvents(config)
        //        .ToListAsync(TestContext.Current.CancellationToken);

        //    changeEvents.Should().ContainSingle(e => e.OldValue == null && e.NewValue == "NotNull");
        //}

        //[Fact]
        //public async Task ToChangeEvents_WithNoChanges_ReturnsEmpty()
        //{
        //    var user = new User { Username = "NoChange", IsAdmin = false, LastUpdatedAt = DateTimeOffset.UtcNow };
        //    dbContext.TestUsers.Add(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        //    var config = new ChangeEventConfiguration(builder =>
        //    {
        //        builder.Configure<UserChange>(uc =>
        //        {
        //            uc.AddProperty(uc => uc.Username);
        //        });
        //    });

        //    var changeEvents = await dbContext.TestUserChanges
        //        .Where(uc => uc.Id == user.Id)
        //        .ToChangeEvents(config)
        //        .ToListAsync(TestContext.Current.CancellationToken);

        //    changeEvents.Should().BeEmpty();
        //}

        //[Fact]
        //public async Task ToChangeEvents_WithMultiplePropertyChangesInSingleSave_ReturnsAllEvents()
        //{
        //    var user = new User { Username = "MultiChange", IsAdmin = false, LastUpdatedAt = DateTimeOffset.UtcNow };
        //    dbContext.TestUsers.Add(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        //    user.Username = "Changed1";
        //    user.IsAdmin = true;
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        //    var config = new ChangeEventConfiguration(builder =>
        //    {
        //        builder.Configure<UserChange>(uc =>
        //        {
        //            uc.AddProperty(uc => uc.Username);
        //            uc.AddProperty(uc => uc.IsAdmin.ToString());
        //        });
        //    });

        //    var changeEvents = await dbContext.TestUserChanges
        //        .Where(uc => uc.Id == user.Id)
        //        .ToChangeEvents(config)
        //        .ToListAsync(TestContext.Current.CancellationToken);

        //    changeEvents.Should().Contain(e => e.Description.Contains("Username"));
        //    changeEvents.Should().Contain(e => e.Description.Contains("IsAdmin"));
        //}

        //[Fact]
        //public async Task ToChangeEvents_GenericOverload_WorksWithChangedByAndChangeSource()
        //{
        //    var user = new User { Username = "GenericUser", IsAdmin = false, LastUpdatedAt = DateTimeOffset.UtcNow };
        //    dbContext.TestUsers.Add(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        //    user.Username = "GenericUserChanged";
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        //    var config = new ChangeEventConfiguration(builder =>
        //    {
        //        builder.Configure<UserChange>(uc =>
        //        {
        //            uc.AddProperty(uc => uc.Username);
        //        });
        //    });

        //    var changeEvents = await dbContext.TestUserChanges
        //        .Where(uc => uc.Id == user.Id)
        //        .ToChangeEvents<string, string>(config)
        //        .ToListAsync(TestContext.Current.CancellationToken);

        //    changeEvents.Should().NotBeEmpty();
        //}

        //[Fact]
        //public async Task ToChangeEvents_WithCustomPropertySelector_Works()
        //{
        //    var user = new User { Username = "CustomSelector", IsAdmin = false, LastUpdatedAt = DateTimeOffset.UtcNow };
        //    dbContext.TestUsers.Add(user);
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
        //    user.Username = "CustomSelectorChanged";
        //    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

        //    var config = new ChangeEventConfiguration(builder =>
        //    {
        //        builder.Configure<UserChange>(uc =>
        //        {
        //            uc.AddProperty(uc => $"User: {uc.Username}");
        //        });
        //    });

        //    var changeEvents = await dbContext.TestUserChanges
        //        .Where(uc => uc.Id == user.Id)
        //        .ToChangeEvents(config)
        //        .ToListAsync(TestContext.Current.CancellationToken);

        //    changeEvents.Should().Contain(e => e.OldValue == "User: CustomSelector" && e.NewValue == "User: CustomSelectorChanged");
        //}

        //[Fact]
        //public void ToChangeEvents_WithInvalidQueryType_ThrowsException()
        //{
        //    var invalidQuery = dbContext.TestUsers.AsQueryable();
        //    Action act = () => invalidQuery.ToChangeEvents();
        //    act.Should().Throw<ChangeEventQueryException>();
        //}
    }
}
