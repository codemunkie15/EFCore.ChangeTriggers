using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSourceEvents;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangeSourceScalar
{
    public class ToChangeEventsTests :
        ToChangeEventsTestBase<ChangeSourceScalarUser, ChangeSourceScalarUserChange, ChangeSourceScalarDbContext, ChangeEvent<ChangeSourceType>>,
        IClassFixture<ToChangeEventsFixture>
    {
        private readonly ScalarChangeSourceProvider changeSourceProvider;

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
            changeSourceProvider = scope.ServiceProvider.GetRequiredService<ScalarChangeSourceProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            changeSourceProvider.CurrentChangeSource.Set(useAsync, ChangeSourceType.Tests);
            return Task.CompletedTask;
        }

        protected override IQueryable<ChangeEvent<ChangeSourceType>> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration)
        {
            return query.ToChangeEvents<ChangeSourceType>(configuration);
        }

        protected override void PopulateAssertProperties(ChangeEvent<ChangeSourceType> changeEvent)
        {
            changeEvent.ChangeSource = changeSourceProvider.CurrentChangeSource.AsyncValue;
        }
    }
}