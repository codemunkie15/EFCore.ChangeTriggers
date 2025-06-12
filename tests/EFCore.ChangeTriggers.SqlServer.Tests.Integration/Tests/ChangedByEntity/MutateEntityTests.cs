using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByEntity
{
    public class MutateEntityTests :
        MutateEntityTestBase<ChangedByEntityUser, ChangedByEntityUserChange, ChangedByEntityDbContext>,
        IClassFixture<MutateEntityFixture>
    {
        private readonly EntityCurrentUserProvider currentUserProvider;

        public MutateEntityTests(MutateEntityFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
        }

        protected override async Task SetChangeContext(bool useAsync)
        {
            currentUserProvider.CurrentUser.Set(useAsync, ChangedByEntityUser.SystemUser);

            var user = new ChangedByEntityUser();
            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesSyncOrAsync(useAsync);

            currentUserProvider.CurrentUser.Set(useAsync, user);
        }

        protected override void Assert(ChangedByEntityUserChange userChange, bool useAsync)
        {
            userChange.ChangedBy.Id.Should().Be(currentUserProvider.CurrentUser.Get(useAsync).Id);
        }

        // TODO: Test when ChangedBy user has been deleted
        /*
            var userChangeEntry = TestScope.DbContext.Entry(userChange);
            userChangeEntry.Property("ChangedById").CurrentValue.Should().Be(user.Id);
        */
    }
}