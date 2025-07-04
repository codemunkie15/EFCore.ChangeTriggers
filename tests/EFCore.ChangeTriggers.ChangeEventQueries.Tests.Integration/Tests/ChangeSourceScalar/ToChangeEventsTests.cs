using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSourceEvents;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangeSourceScalar.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangeSourceScalar
{
    public class ToChangeEventsTests :
        ToChangeEventsTestBase<ChangeSourceScalarUser, ChangeSourceScalarUserChange, ChangeSourceScalarDbContext, ChangeEvent<ChangeSourceType>>,
        IClassFixture<ToChangeEventsFixture>
    {
        private readonly ToChangeEventsFixture fixture;
        private ScalarChangeSourceProvider changeSourceProvider;

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
            this.fixture = fixture;
        }

        protected override void SetupServices(IServiceProvider services)
        {
            base.SetupServices(services);
            changeSourceProvider = services.GetRequiredService<ScalarChangeSourceProvider>();
        }

        protected override Task SetChangeContext(bool useAsync)
        {
            changeSourceProvider.CurrentChangeSource.Set(useAsync, ChangeSourceType.Tests);
            return Task.CompletedTask;
        }

        protected override IQueryable<ChangeEvent<ChangeSourceType>> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration)
        {
            return configuration is null
                ? query.ToChangeEvents<ChangeSourceType>()
                : query.ToChangeEvents<ChangeSourceType>(configuration);
        }

        protected override IServiceProvider BuildServiceProvider(Action<ChangeEventsDbContextOptionsBuilder> options = null)
        {
            return new ServiceCollection()
                .AddChangeSourceScalar(fixture.GetConnectionString(), options)
                .BuildServiceProvider();
        }

        protected override void PopulateAssertProperties(ChangeEvent<ChangeSourceType> changeEvent)
        {
            changeEvent.ChangeSource = changeSourceProvider.CurrentChangeSource.AsyncValue;
        }
    }
}