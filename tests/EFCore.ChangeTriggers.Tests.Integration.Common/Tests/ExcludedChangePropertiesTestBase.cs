using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using FluentAssertions;
using Xunit;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Tests
{
    public abstract class ExcludedChangePropertiesTestBase : UserTestBase<UserWithPassword, UserWithPasswordChange, ExcludedChangePropertiesDbContext>
    {
        protected ExcludedChangePropertiesTestBase(IServiceProvider services) : base(services)
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateProperty_ExcludedFromChangeEntity_DoesNotGenerateChange(bool useAsync)
        {
            var user = new UserWithPassword
            {
                Username = "Test",
                PasswordHash = "InitialHash"
            };

            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesSyncOrAsync(useAsync);

            user.PasswordHash = "ModifiedHash";

            await dbContext.SaveChangesSyncOrAsync(useAsync);

            var changes = dbContext.TestUserChanges.Where(c => c.Id == user.Id && c.OperationType == OperationType.Update).ToList();

            changes.Should().BeEmpty();
        }
    }
}
