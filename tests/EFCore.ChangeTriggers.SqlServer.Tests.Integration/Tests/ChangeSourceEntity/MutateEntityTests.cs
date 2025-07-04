using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangeSourceEntity
{
    public class MutateEntityTests :
        MutateEntityTestBase<ChangeSourceEntityUser, ChangeSourceEntityUserChange, ChangeSourceEntityDbContext>,
        IClassFixture<MutateEntityFixture>
    {
        private readonly EntityChangeSourceProvider changeSourceProvider;

        public MutateEntityTests(MutateEntityFixture fixture) : base(fixture.Services)
        {
            changeSourceProvider = scope.ServiceProvider.GetRequiredService<EntityChangeSourceProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            changeSourceProvider.CurrentChangeSource.Set(useAsync, new ChangeSource { Id = 2 });
            return Task.CompletedTask;
        }

        protected override void Assert(ChangeSourceEntityUserChange userChange, bool useAsync)
        {
            userChange.ChangeSource.Id.Should().Be(changeSourceProvider.CurrentChangeSource.Get(useAsync).Id);
        }

        // TODO: Test when ChangedBy user has been deleted
        /*
            var userChangeEntry = TestScope.DbContext.Entry(userChange);
            userChangeEntry.Property("ChangedById").CurrentValue.Should().Be(user.Id);
        */
    }
}