using EFCore.ChangeTriggers.ChangeEventQueries.ChangedByEvents;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByEntity
{
    public class ToChangeEventsTests :
        ToChangeEventsTestBase<ChangedByEntityUser, ChangedByEntityUserChange, ChangedByEntityDbContext, ChangeEvent<ChangedByEntityUser>>,
        IClassFixture<ToChangeEventsFixture>
    {
        private readonly EntityCurrentUserProvider currentUserProvider;

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
        }

        protected override async Task SetChangeContext(bool useAsync)
        {
            var systemUser = await dbContext.TestUsers.SingleAsync(u => u.Id == ChangedByEntityUser.SystemUser.Id);
            currentUserProvider.CurrentUser.Set(useAsync, systemUser);
        }

        protected override IQueryable<ChangeEvent<ChangedByEntityUser>> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration)
        {
            return query.ToChangeEvents<ChangedByEntityUser>(configuration);
        }

        protected override void PopulateAssertProperties(ChangeEvent<ChangedByEntityUser> changeEvent)
        {
            changeEvent.ChangedBy = currentUserProvider.CurrentUser.AsyncValue;
        }
    }
}