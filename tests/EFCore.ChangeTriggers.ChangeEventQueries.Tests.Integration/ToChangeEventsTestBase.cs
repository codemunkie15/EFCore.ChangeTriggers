using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration
{
    public abstract class ToChangeEventsTestBase<TUser, TUserChange, TDbContext, TChangeEvent> : UserTestBase<TUser, TUserChange, TDbContext>
        where TUser : UserBase, ITracked<TUserChange>, new()
        where TUserChange : UserBase, IChange<TUser>, IHasChangeId
        where TDbContext : TestDbContext<TUser, TUserChange>
        where TChangeEvent : ChangeEvent, new()
    {
        protected ToChangeEventsTestBase(IServiceProvider services) : base(services)
        {
        }

        protected abstract IQueryable<TChangeEvent> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration);

        protected virtual void PopulateAssertProperties(TChangeEvent changeEvent) { }

        // TODO: Single property updates vs multiple property updates in same transaction

        [Fact]
        public async Task ToChangeEvents_WithMultipleChanges_InSameUpdate_ReturnsCorrectChangeEvents()
        {
            var startingUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true
            };

            var newUser = new TUser
            {
                Username = "Test User Name Change",
                IsAdmin = false
            };

            await RunTest(startingUser,
                act: async createdUser =>
                {
                    createdUser.Username = newUser.Username;
                    createdUser.IsAdmin = newUser.IsAdmin;

                    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
                },
                config: new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<TUserChange>(uc =>
                    {
                        uc.AddProperty(uc => uc.Username);
                        uc.AddProperty(uc => uc.IsAdmin.ToString());
                    });
                }),
                expectedChangeEvents:
                [
                    new()
                    {
                        Description = $"{nameof(UserBase.Username)} changed",
                        OldValue = startingUser.Username,
                        NewValue = newUser.Username,
                    },
                    new()
                    {
                        Description = $"{nameof(UserBase.IsAdmin)} changed",
                        OldValue = startingUser.IsAdmin.ToString(),
                        NewValue = newUser.IsAdmin.ToString(),
                    }
                ]);
        }

        [Fact]
        public async Task ToChangeEvents_WithSingleChanges_InMultipleUpdates_ReturnsCorrectChangeEvents()
        {
            var startingUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true
            };

            var newUser = new TUser
            {
                Username = "Test User Name Change",
                IsAdmin = false
            };

            await RunTest(startingUser,
                act: async createdUser =>
                {
                    createdUser.Username = newUser.Username;

                    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

                    createdUser.IsAdmin = newUser.IsAdmin;

                    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
                },
                config: new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<TUserChange>(uc =>
                    {
                        uc.AddProperty(uc => uc.Username);
                        uc.AddProperty(uc => uc.IsAdmin.ToString());
                        uc.AddProperty(uc => uc.LastUpdatedAt.ToString());
                    });
                }),
                expectedChangeEvents:
                [
                    new()
                    {
                        Description = $"{nameof(UserBase.Username)} changed",
                        OldValue = startingUser.Username,
                        NewValue = newUser.Username,
                    },
                    new()
                    {
                        Description = $"{nameof(UserBase.IsAdmin)} changed",
                        OldValue = startingUser.IsAdmin.ToString(),
                        NewValue = newUser.IsAdmin.ToString(),
                    }
                ]);
        }

        [Fact]
        public async Task ToChangeEvents_WithMultipleChanges_InMultipleUpdates_ReturnsCorrectChangeEvents()
        {
            var startingUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true,
                LastUpdatedAt = DateTimeOffset.UtcNow,
                DateOfBirth = "2000-01-01"
            };

            var newUser = new TUser
            {
                Username = "Test User Name Change",
                IsAdmin = false,
                LastUpdatedAt = DateTimeOffset.UtcNow.AddHours(1),
                DateOfBirth = "2000-02-02"
            };

            await RunTest(startingUser,
                act: async createdUser =>
                {
                    createdUser.Username = newUser.Username;
                    createdUser.IsAdmin = newUser.IsAdmin;

                    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

                    createdUser.LastUpdatedAt = newUser.LastUpdatedAt;
                    createdUser.DateOfBirth = newUser.DateOfBirth;

                    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
                },
                config: new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<TUserChange>(uc =>
                    {
                        uc.AddProperty(uc => uc.Username);
                        uc.AddProperty(uc => uc.IsAdmin.ToString());
                        uc.AddProperty(uc => uc.LastUpdatedAt.ToString());
                        uc.AddProperty(uc => uc.DateOfBirth);
                    });
                }),
                expectedChangeEvents:
                [
                    new()
                    {
                        Description = $"{nameof(UserBase.Username)} changed",
                        OldValue = startingUser.Username,
                        NewValue = newUser.Username,
                    },
                    new()
                    {
                        Description = $"{nameof(UserBase.IsAdmin)} changed",
                        OldValue = startingUser.IsAdmin.ToString(),
                        NewValue = newUser.IsAdmin.ToString(),
                    },
                    new()
                    {
                        Description = $"{nameof(UserBase.LastUpdatedAt)} changed",
                        OldValue = startingUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz"), // SQL Server format
                        NewValue = newUser.LastUpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz"), // SQL Server format
                    },
                    new()
                    {
                        Description = $"{nameof(UserBase.DateOfBirth)} changed",
                        OldValue = startingUser.DateOfBirth.ToString(),
                        NewValue = newUser.DateOfBirth.ToString(),
                    },
                ]);
        }

        [Fact]
        public async Task ToChangeEvents_WithInserts_ReturnsCorrectChangeEvents()
        {
            var startingUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true
            };

            await RunTest(startingUser,
                act: createdUser => Task.CompletedTask,
                config: new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<TUserChange>(uc =>
                    {
                        uc.AddInserts();
                    });
                }),
                expectedChangeEvents:
                [
                    new()
                    {
                        Description = "Entity created",
                        OldValue = null,
                        NewValue = null,
                    }
                ]);
        }

        [Fact]
        public async Task ToChangeEvents_WithDeletes_ReturnsCorrectChangeEvents()
        {
            var startingUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true
            };

            await RunTest(startingUser,
                act: async createdUser =>
                {
                    dbContext.TestUsers.Remove(createdUser);
                    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
                },
                config: new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<TUserChange>(uc =>
                    {
                        uc.AddDeletes();
                    });
                }),
                expectedChangeEvents:
                [
                    new()
                    {
                        Description = "Entity deleted",
                        OldValue = null,
                        NewValue = null,
                    }
                ]);
        }

        protected async Task RunTest(
            TUser startingUser,
            Func<TUser, Task> act,
            ChangeEventConfiguration config,
            List<TChangeEvent> expectedChangeEvents)
        {
            // Arrange
            await SetChangeContext(true);

            dbContext.TestUsers.Add(startingUser);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

            var createdUser = await dbContext.TestUsers
                .SingleAsync(u => u.Id == startingUser.Id, TestContext.Current.CancellationToken);

            // Act
            await act(createdUser);

            var changes = userReadRepository.GetUserChanges()
                .Where(uc => uc.Id == createdUser.Id);

            var changeEvents = await ToChangeEvents(changes, config)
                .ToListAsync(TestContext.Current.CancellationToken);

            // Assert
            expectedChangeEvents.ForEach(PopulateAssertProperties);

            changeEvents.Should().BeEquivalentTo(expectedChangeEvents, options => options
                .Excluding(ce => ce.ChangedAt));

            changeEvents.Should().AllSatisfy(ce =>
            {
               ce.ChangedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
            });
        }
    }
}