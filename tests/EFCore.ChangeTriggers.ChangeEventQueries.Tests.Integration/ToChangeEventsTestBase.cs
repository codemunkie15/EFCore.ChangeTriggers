using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration
{
    public abstract class ToChangeEventsTestBase<TUser, TUserChange, TDbContext> : UserTestBase<TUser, TUserChange, TDbContext>
        where TUser : UserBase, ITracked<TUserChange>, new()
        where TUserChange : UserBase, IChange<TUser>, IHasChangeId
        where TDbContext : TestDbContext<TUser, TUserChange>
    {
        protected ToChangeEventsTestBase(IServiceProvider services) : base(services)
        {
        }

        [Fact]
        public async Task ToChangeEvents_WithSimpleProperties_ReturnsCorrectChangeEvents()
        {
            await SetChangeContext(true);

            var oldUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true,
                LastUpdatedAt = DateTimeOffset.UtcNow
            };

            var newUser = new TUser
            {
                Username = "Test User Name Change",
                IsAdmin = false,
                LastUpdatedAt = DateTimeOffset.UtcNow.AddHours(1)
            };

            var user = new TUser
            {
                Username = oldUser.Username,
                IsAdmin = oldUser.IsAdmin,
                LastUpdatedAt = oldUser.LastUpdatedAt
            };

            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            user.Username = newUser.Username;
            user.IsAdmin = newUser.IsAdmin;

            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            user.LastUpdatedAt = newUser.LastUpdatedAt;

            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var changeEvents = await userReadRepository.GetUserChanges()
                .Where(uc => uc.Id == user.Id)
                .ToChangeEvents(new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<TUserChange>(uc =>
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
                    Description = $"{nameof(UserBase.Username)} changed",
                    OldValue = oldUser.Username,
                    NewValue = newUser.Username,
                },
                new ChangeEvent
                {
                    Description = $"{nameof(UserBase.IsAdmin)} changed",
                    OldValue = oldUser.IsAdmin.ToString(),
                    NewValue = newUser.IsAdmin.ToString(),
                },
                new ChangeEvent
                {
                    Description = $"{nameof(UserBase.LastUpdatedAt)} changed",
                    OldValue = oldUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz"), // SQL Server format
                    NewValue = newUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz"), // SQL Server format
                }
            ], options => options.Excluding(ce => ce.ChangedAt));
        }
    }
}