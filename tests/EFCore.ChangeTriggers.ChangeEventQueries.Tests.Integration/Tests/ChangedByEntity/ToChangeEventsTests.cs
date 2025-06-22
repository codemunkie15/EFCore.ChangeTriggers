using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByEntity
{
    public class ToChangeEventsTests : ToChangeEventsTestBase<ChangedByEntityUser, ChangedByEntityUserChange, ChangedByEntityDbContext>, IClassFixture<ToChangeEventsFixture>
    {
        private readonly EntityCurrentUserProvider currentUserProvider;

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            currentUserProvider.CurrentUser.Set(useAsync, ChangedByEntityUser.SystemUser);
            return Task.CompletedTask;
        }
    }
}