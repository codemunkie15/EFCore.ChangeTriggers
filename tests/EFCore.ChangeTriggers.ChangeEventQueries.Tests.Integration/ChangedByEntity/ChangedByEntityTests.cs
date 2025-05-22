using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Helpers;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.ChangedByEntity
{
    public class ChangedByEntityTests : IClassFixture<ChangedByEntityFixture>
    {
        private readonly ChangedByEntityFixture fixture;
        private readonly ChangedByEntityTestHelper testHelper;

        public ChangedByEntityTests(ChangedByEntityFixture fixture)
        {
            this.fixture = fixture;
            this.testHelper = new(fixture.Services);
        }

        [Fact]
        public async Task ToChangeEvents_WithSimpleProperties_ReturnsCorrectChangeEvents()
        {
            testHelper.CurrentUserProvider.CurrentUserAsync = ChangedByEntityUser.SystemUser;

            var oldUser = new ChangedByEntityUser
            {
                Username = "Test User",
                IsAdmin = true,
                LastUpdatedAt = DateTimeOffset.UtcNow
            };

            var newUser = new ChangedByEntityUser
            {
                Username = "Test User Name Change",
                IsAdmin = false,
                LastUpdatedAt = DateTimeOffset.UtcNow.AddHours(1)
            };

            var user = new ChangedByEntityUser
            {
                Username = oldUser.Username,
                IsAdmin = oldUser.IsAdmin,
                LastUpdatedAt = oldUser.LastUpdatedAt
            };

            testHelper.DbContext.TestUsers.Add(user);
            await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            user.Username = newUser.Username;
            user.IsAdmin = newUser.IsAdmin;

            await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            user.LastUpdatedAt = newUser.LastUpdatedAt;

            await testHelper.DbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var changeEvents = await testHelper.DbContext.TestUserChanges
                .Where(uc => uc.Id == user.Id)
                .ToChangeEvents(new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<ChangedByEntityUserChange>(uc =>
                    {
                        uc.AddProperty(uc => uc.Username);
                        uc.AddProperty(uc => uc.IsAdmin.ToString());
                        uc.AddProperty(uc => uc.LastUpdatedAt.ToString());
                    });
                }))
                .ToListAsync(TestContext.Current.CancellationToken);

            changeEvents.Should().BeEquivalentTo(
            [
                new ChangeEvent
                { 
                    Description = $"{nameof(ChangedByEntityUser.Username)} changed",
                    OldValue = oldUser.Username,
                    NewValue = newUser.Username
                },
                new ChangeEvent
                {
                    Description = $"{nameof(ChangedByEntityUser.IsAdmin)} changed",
                    OldValue = oldUser.IsAdmin.ToString(),
                    NewValue = newUser.IsAdmin.ToString()
                },
                new ChangeEvent
                {
                    Description = $"{nameof(ChangedByEntityUser.LastUpdatedAt)} changed",
                    OldValue = oldUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz"), // SQL Server format
                    NewValue = newUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz") }, // SQL Server format
            ], options => options.Excluding(ce => ce.ChangedAt));
        }

        // Test exceptions

        // Test passed in config

        // Test DbContext config

        // Test include inserts/deletes
    }
}
