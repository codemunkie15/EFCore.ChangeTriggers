using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Tests
{
    public abstract class UserTestBase<TUser, TUserChange, TDbContext> : TestBase<TDbContext>
        where TUser : UserBase, ITracked<TUserChange>, new()
        where TUserChange : UserBase, IChange<TUser>, IHasChangeId
        where TDbContext : TestDbContext<TUser, TUserChange>
    {
        protected IUserReadRepository<TUser, TUserChange> userReadRepository;

        protected UserTestBase(IServiceProvider services) : base(services)
        {
        }

        protected override void SetupServices(IServiceProvider services)
        {
            userReadRepository = services.GetRequiredService<IUserReadRepository<TUser, TUserChange>>();
        }

        protected virtual Task SetChangeContext(bool useAsync) => Task.CompletedTask;

        protected virtual void Assert(TUserChange userChange, bool useAsync) { }

        protected void AssertBase(TUserChange userChange, TUser user, OperationType operationType)
        {
            userChange.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
            userChange.OperationType.Should().Be(operationType);

            if (operationType == OperationType.Delete)
            {
                userChange.TrackedEntity.Should().BeNull();
            }
            else
            {
                userChange.TrackedEntity.Should().NotBeNull();
                userChange.TrackedEntity.Id.Should().Be(user.Id);
            }
        }
    }
}
