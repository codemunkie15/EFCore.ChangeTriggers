using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using FluentAssertions;
using Xunit;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Tests
{
    public abstract class MigrateDatabaseTestBase<TUser, TUserChange, TDbContext> : UserTestBase<TUser, TUserChange, TDbContext>
        where TUser : UserBase, ITracked<TUserChange>, new()
        where TUserChange : UserBase, IChange<TUser>, IHasChangeId
        where TDbContext : TestDbContext<TUser, TUserChange>
    {
        protected MigrateDatabaseTestBase(IServiceProvider services) : base(services)
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task MigrateDatabase_InsertsChangeEntity_WithCorrectProperties(bool useAsync)
        {
            await SetChangeContext(useAsync);

            await dbContext.Database.MigrateSyncOrAsync(useAsync);

            var testUsers = userReadRepository.GetUsers().ToList();

            testUsers.Should().AllSatisfy(u =>
            {
                var userChange = u.Changes.Should().ContainSingle().Which;

                AssertBase(userChange, u, OperationType.Insert);
                Assert(userChange, useAsync);
            });
        }
    }
}