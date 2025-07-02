using EFCore.ChangeTriggers.ChangeEventQueries.ChangedByEvents;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByScalar
{
    public class ToChangeEventsTests :
        ToChangeEventsTestBase<ChangedByScalarUser, ChangedByScalarUserChange, ChangedByScalarDbContext, ChangeEvent<string>>,
        IClassFixture<ToChangeEventsFixture>
    {
        private readonly ToChangeEventsFixture fixture;
        private ScalarCurrentUserProvider currentUserProvider;

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
            this.fixture = fixture;
        }

        protected override void SetupServices(IServiceProvider services)
        {
            base.SetupServices(services);
            currentUserProvider = services.GetRequiredService<ScalarCurrentUserProvider>();
        }

        protected override async Task SetChangeContext(bool useAsync)
        {
            var systemUser = await dbContext.TestUsers.SingleAsync(u => u.Id == ChangedByScalarUser.SystemUser.Id);
            currentUserProvider.CurrentUser.Set(useAsync, systemUser.Username);
        }

        protected override IQueryable<ChangeEvent<string>> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration)
        {
            return configuration is null
                ? query.ToChangeEvents<string>()
                : query.ToChangeEvents<string>(configuration);
        }

        protected override IServiceProvider BuildServiceProvider(Action<ChangeEventsDbContextOptionsBuilder> options = null)
        {
            return new ServiceCollection()
                .AddChangedByScalar(fixture.GetConnectionString(), options)
                .BuildServiceProvider();
        }

        protected override void PopulateAssertProperties(ChangeEvent<string> changeEvent)
        {
            changeEvent.ChangedBy = currentUserProvider.CurrentUser.AsyncValue;
        }
    }
}