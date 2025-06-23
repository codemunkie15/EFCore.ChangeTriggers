using EFCore.ChangeTriggers.ChangeEventQueries.ChangedByEvents;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByScalar
{
    public class ToChangeEventsTests :
        ToChangeEventsTestBase<ChangedByScalarUser, ChangedByScalarUserChange, ChangedByScalarDbContext, ChangeEvent<string>>,
        IClassFixture<ToChangeEventsFixture>
    {
        private readonly ScalarCurrentUserProvider currentUserProvider;

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
            currentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();
        }

        protected override async Task SetChangeContext(bool useAsync)
        {
            var systemUser = await dbContext.TestUsers.SingleAsync(u => u.Id == ChangedByScalarUser.SystemUser.Id);
            currentUserProvider.CurrentUser.Set(useAsync, systemUser.Username);
        }

        protected override IQueryable<ChangeEvent<string>> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration)
        {
            return query.ToChangeEvents<string>(configuration);
        }

        protected override void PopulateAssertProperties(ChangeEvent<string> changeEvent)
        {
            changeEvent.ChangedBy = currentUserProvider.CurrentUser.AsyncValue;
        }
    }
}