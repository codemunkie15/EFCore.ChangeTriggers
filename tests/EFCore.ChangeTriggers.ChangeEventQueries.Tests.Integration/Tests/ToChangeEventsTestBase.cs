using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests
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

        protected abstract IServiceProvider BuildServiceProvider(Action<ChangeEventsDbContextOptionsBuilder> options = null);

        protected virtual void PopulateAssertProperties(TChangeEvent changeEvent) { }

        public static IEnumerable<object[]> TestConfigurationData => [
            [ConfigurationProviderMode.DbContext],
            [ConfigurationProviderMode.Parameter]
        ];

        [Theory]
        [MemberData(nameof(TestConfigurationData))]
        public async Task ToChangeEvents_WithMultipleChanges_InSingleSave_ReturnsCorrectChangeEvents(ConfigurationProviderMode configurationProviderMode)
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

            await RunTest(configurationProviderMode, startingUser,
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
                serviceProvider: BuildServiceProvider(options => options.ConfigurationsAssembly(Assembly.GetExecutingAssembly())),
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

        [Theory]
        [MemberData(nameof(TestConfigurationData))]
        public async Task ToChangeEvents_WithSingleChanges_InMultipleSaves_ReturnsCorrectChangeEvents(ConfigurationProviderMode configurationProviderMode)
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

            await RunTest(configurationProviderMode, startingUser,
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
                serviceProvider: BuildServiceProvider(options => options.ConfigurationsAssembly(Assembly.GetExecutingAssembly())),
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

        [Theory]
        [MemberData(nameof(TestConfigurationData))]
        public async Task ToChangeEvents_WithMultipleChanges_InMultipleSaves_ReturnsCorrectChangeEvents(ConfigurationProviderMode configurationProviderMode)
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

            await RunTest(configurationProviderMode, startingUser,
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
                serviceProvider: BuildServiceProvider(options => options.ConfigurationsAssembly(Assembly.GetExecutingAssembly())),
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

            await RunTest(ConfigurationProviderMode.Parameter, startingUser,
                act: createdUser => Task.CompletedTask,
                config: new ChangeEventConfiguration(builder =>
                {
                    builder.Configure<TUserChange>(uc =>
                    {
                        uc.AddInserts();
                    });
                }),
                serviceProvider: null,
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

            await RunTest(ConfigurationProviderMode.Parameter, startingUser,
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
                serviceProvider: null,
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

        //[Fact]
        public async Task ToChangeEvents_WithGlobalInserts_ReturnsCorrectChangeEvents()
        {
            var startingUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true
            };

            await RunTest(ConfigurationProviderMode.DbContext, startingUser,
                act: createdUser => Task.CompletedTask,
                config: null,
                serviceProvider: BuildServiceProvider(options => options.IncludeInserts()),
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

        //[Fact]
        public async Task ToChangeEvents_WithGlobalDeletes_ReturnsCorrectChangeEvents()
        {
            // Might want to configure the service provider but still pass config via parameter
            var startingUser = new TUser
            {
                Username = "Test User",
                IsAdmin = true
            };

            await RunTest(ConfigurationProviderMode.DbContext, startingUser,
                act: async createdUser =>
                {
                    dbContext.TestUsers.Remove(createdUser);
                    await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
                },
                config: null,
                serviceProvider: BuildServiceProvider(options => options.IncludeDeletes()),
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
            ConfigurationProviderMode configurationProviderMode,
            TUser startingUser,
            Func<TUser, Task> act,
            ChangeEventConfiguration config,
            IServiceProvider serviceProvider,
            List<TChangeEvent> expectedChangeEvents)
        {
            if (configurationProviderMode == ConfigurationProviderMode.DbContext)
            {
                UseServiceProvider(serviceProvider);
                config = null;
            }

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