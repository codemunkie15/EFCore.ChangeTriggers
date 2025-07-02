using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests
{
    public class ConfigurationTests : TestBase<TestDbContext>, IClassFixture<ConfigurationFixture>
    {
        public ConfigurationTests(ConfigurationFixture fixture) : base(fixture.Services)
        {
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

        [Fact]
        public void ToChangeEvents_WithInvalidPropertySelectorConfiguration_ThrowsException()
        {
            var changeEvents = () => dbContext.TestUserChanges.ToChangeEvents(new ChangeEventConfiguration(builder =>
            {
                builder.Configure<UserChange>(uc =>
                {
                    uc.AddProperty(uc => "");
                });
            }));

            changeEvents.Should()
                .Throw<ChangeEventConfigurationException>()
                .WithMessage("The lambda expression does not refer to a valid member on the type UserChange.");
        }

        [Fact]
        public void ToChangeEvents_WithInvalidQueryType_ThrowsException()
        {
            var changeEvents = () => dbContext.TestUsers.ToChangeEvents();

            changeEvents.Should()
                .Throw<ChangeEventQueryException>()
                .WithMessage("The provided IQueryable must be an IQueryable<IChange>.");
        }
    }
}