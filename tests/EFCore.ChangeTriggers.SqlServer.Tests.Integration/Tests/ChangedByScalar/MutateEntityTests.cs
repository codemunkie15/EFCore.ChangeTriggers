using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.ChangedByScalar
{
    public class MutateEntityTests :
        MutateEntityTestBase<ChangedByScalarUser, ChangedByScalarUserChange, ChangedByScalarDbContext>,
        IClassFixture<MutateEntityFixture>
    {
        private readonly ScalarCurrentUserProvider currentUserProvider;

        public MutateEntityTests(MutateEntityFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();
        }

        protected override async Task SetChangeContext(bool useAsync)
        {
            currentUserProvider.CurrentUser.Set(useAsync, ChangedByScalarUser.SystemUser.Username);

            var user = new ChangedByScalarUser();
            dbContext.TestUsers.Add(user);
            await dbContext.SaveChangesSyncOrAsync(useAsync);

            currentUserProvider.CurrentUser.Set(useAsync, user.Username);
        }

        protected override void Assert(ChangedByScalarUserChange userChange, bool useAsync)
        {
            userChange.ChangedBy.Should().Be(currentUserProvider.CurrentUser.Get(useAsync));
        }

        // TODO: Test when ChangedBy user has been deleted
        /*
            var userChangeEntry = TestScope.DbContext.Entry(userChange);
            userChangeEntry.Property("ChangedById").CurrentValue.Should().Be(user.Id);
        */
    }
}