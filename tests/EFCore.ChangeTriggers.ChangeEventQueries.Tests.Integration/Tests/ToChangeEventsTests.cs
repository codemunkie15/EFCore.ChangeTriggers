//using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
//using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
//using EFCore.ChangeTriggers.Tests.Integration.Common.Helpers;
//using FluentAssertions;

//namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests
//{
//    public class ToChangeEventsTests : IClassFixture<ToChangeEventsTestFixture>
//    {
//        private readonly ToChangeEventsTestFixture fixture;
//        private readonly TestHelper testHelper;

//        public ToChangeEventsTests(ToChangeEventsTestFixture fixture)
//        {
//            this.fixture = fixture;
//            testHelper = new(fixture.Services);
//        }

//        //[Fact]
//        //public async Task ToChangeEvents_WithSimpleProperties_ReturnsCorrectChangeEvents()
//        //{
//        //    testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

//        //    var oldUser = new User
//        //    {
//        //        Username = "Test User",
//        //        IsAdmin = true,
//        //        LastUpdatedAt = DateTimeOffset.UtcNow
//        //    };

//        //    var newUser = new User
//        //    {
//        //        Username = "Test User Name Change",
//        //        IsAdmin = false,
//        //        LastUpdatedAt = DateTimeOffset.UtcNow.AddHours(1)
//        //    };

//        //    var user = new User
//        //    {
//        //        Username = oldUser.Username,
//        //        IsAdmin = oldUser.IsAdmin,
//        //        LastUpdatedAt = oldUser.LastUpdatedAt
//        //    };

//        //    testHelper.DbContext.TestUsers.Add(user);
//        //    await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

//        //    user.Username = newUser.Username;
//        //    user.IsAdmin = newUser.IsAdmin;

//        //    await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

//        //    user.LastUpdatedAt = newUser.LastUpdatedAt;

//        //    await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

//        //    var changeEvents = await testHelper.DbContext.TestUserChanges
//        //        .Where(uc => uc.Id == user.Id)
//        //        .ToChangeEvents(new ChangeEventConfiguration(builder =>
//        //        {
//        //            builder.Configure<UserChange>(uc =>
//        //            {
//        //                uc.AddProperty(uc => uc.Username);
//        //                uc.AddProperty(uc => uc.IsAdmin.ToString());
//        //                uc.AddProperty(uc => uc.LastUpdatedAt.ToString());
//        //            });
//        //        }))
//        //        .ToListAsync(TestContext.Current.CancellationToken);

//        //    changeEvents.Should().BeEquivalentTo(
//        //    [
//        //        new ChangeEvent
//        //        { 
//        //            Description = $"{nameof(User.Username)} changed",
//        //            OldValue = oldUser.Username,
//        //            NewValue = newUser.Username,

//        //        },
//        //        new ChangeEvent
//        //        {
//        //            Description = $"{nameof(User.IsAdmin)} changed",
//        //            OldValue = oldUser.IsAdmin.ToString(),
//        //            NewValue = newUser.IsAdmin.ToString(),
//        //        },
//        //        new ChangeEvent
//        //        {
//        //            Description = $"{nameof(User.LastUpdatedAt)} changed",
//        //            OldValue = oldUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz"), // SQL Server format
//        //            NewValue = newUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz"), // SQL Server format
//        //        }
//        //    ], options => options.Excluding(ce => ce.ChangedAt));
//        //}

//        [Fact]
//        // Test exceptions
//        public void ToChangeEvents_WithNoConfiguration_ThrowsException()
//        {
//            var changeEvents = () => testHelper.DbContext.TestUserChanges.ToChangeEvents();

//            changeEvents.Should()
//                .Throw<ChangeEventQueryException>()
//                .WithMessage("No configuration found.*");
//        }

//        [Fact]
//        public void ToChangeEvents_WithNoConfigurationForEntity_ThrowsException()
//        {
//            var changeEvents = () => testHelper.DbContext.TestUserChanges.ToChangeEvents(new ChangeEventConfiguration());

//            changeEvents.Should()
//                .Throw<ChangeEventQueryException>()
//                .WithMessage("No configuration found for entity type UserChange.");
//        }

//        // Test passed in config

//        // Test DbContext config

//        // Test include inserts/deletes
//    }
//}
