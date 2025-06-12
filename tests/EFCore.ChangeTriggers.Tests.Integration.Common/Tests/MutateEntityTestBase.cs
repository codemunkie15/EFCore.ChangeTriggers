using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using FluentAssertions;
using Xunit;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Tests
{
    public abstract class MutateEntityTestBase<TUser, TUserChange, TDbContext> : UserTestBase<TUser, TUserChange, TDbContext>
        where TUser : UserBase, ITracked<TUserChange>, new()
        where TUserChange : UserBase, IChange<TUser>, IHasChangeId
        where TDbContext : TestDbContext<TUser, TUserChange>
    {
        protected MutateEntityTestBase(IServiceProvider services) : base(services)
        {
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task AddEntity_InsertsChangeEntity_WithCorrectProperties(bool useAsync)
        {
            await RunMutateEntityTest(
                useAsync,
                OperationType.Insert,
                async () =>
                {
                    // Act
                    var user = new TUser();
                    dbContext.TestUsers.Add(user);
                    await dbContext.SaveChangesSyncOrAsync(useAsync);
                    return user;
                });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task UpdateEntity_InsertsChangeEntity_WithCorrectProperties(bool useAsync)
        {
            await RunMutateEntityTest(
                useAsync,
                OperationType.Update,
                async () =>
                {
                    // Act
                    var user = new TUser();
                    dbContext.TestUsers.Add(user);
                    await dbContext.SaveChangesSyncOrAsync(useAsync);

                    user.Username = "Modified";
                    await dbContext.SaveChangesSyncOrAsync(useAsync);

                    return user;
                });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteEntity_InsertsChangeEntity_WithCorrectProperties(bool useAsync)
        {
            await RunMutateEntityTest(
                useAsync,
                OperationType.Delete,
                async () =>
                {
                    // Act
                    var user = new TUser();
                    dbContext.TestUsers.Add(user);
                    await dbContext.SaveChangesSyncOrAsync(useAsync);

                    dbContext.TestUsers.Remove(user);
                    await dbContext.SaveChangesSyncOrAsync(useAsync);

                    return user;
                });
        }

        private async Task RunMutateEntityTest(bool useAsync, OperationType operationType, Func<Task<TUser>> userActions)
        {
            // Arrange
            await SetChangeContext(useAsync);

            // Act
            var user = await userActions();

            // Assert
            var userChanges = userReadRepository.GetUserChanges()
                .Where(uc => uc.Id == user.Id && uc.OperationType == operationType)
                .ToList();

            var userChange = userChanges.Should().ContainSingle().Which;

            AssertBase(userChange, user, operationType);
            Assert(userChange, useAsync);
        }
    }
}