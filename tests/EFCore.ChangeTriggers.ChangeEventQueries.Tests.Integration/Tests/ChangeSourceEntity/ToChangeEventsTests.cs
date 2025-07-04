using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSourceEvents;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure;
using EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangeSourceEntity.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangeSourceEntity
{
    public class ToChangeEventsTests :
        ToChangeEventsTestBase<ChangeSourceEntityUser, ChangeSourceEntityUserChange, ChangeSourceEntityDbContext, ChangeEvent<ChangeSource>>,
        IClassFixture<ToChangeEventsFixture>
    {
        private readonly ToChangeEventsFixture fixture;
        private EntityChangeSourceProvider changeSourceProvider;

        public ToChangeEventsTests(ToChangeEventsFixture fixture) : base(fixture.Services)
        {
            this.fixture = fixture;
        }

        protected override void SetupServices(IServiceProvider services)
        {
            base.SetupServices(services);
            changeSourceProvider = services.GetRequiredService<EntityChangeSourceProvider>();
        }

        protected override async Task SetChangeContext(bool useAsync)
        {
            var changeSource = await dbContext.ChangeSources.SingleAsync(u => u.Name == "Tests");
            changeSourceProvider.CurrentChangeSource.Set(useAsync, changeSource);
        }

        protected override IQueryable<ChangeEvent<ChangeSource>> ToChangeEvents(IQueryable query, ChangeEventConfiguration configuration)
        {
            return configuration is null
                ? query.ToChangeEvents<ChangeSource>()
                : query.ToChangeEvents<ChangeSource>(configuration);
        }

        protected override IServiceProvider BuildServiceProvider(Action<ChangeEventsDbContextOptionsBuilder> options = null)
        {
            return new ServiceCollection()
                .AddChangeSourceEntity(fixture.GetConnectionString(), options)
                .BuildServiceProvider();
        }

        protected override void PopulateAssertProperties(ChangeEvent<ChangeSource> changeEvent)
        {
            changeEvent.ChangeSource = changeSourceProvider.CurrentChangeSource.AsyncValue;
        }
    }
}