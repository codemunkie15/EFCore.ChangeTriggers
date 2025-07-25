﻿using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;
using EFCore.ChangeTriggers.Tests.Integration.Common.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Tests.Integration.Tests.ChangedByEntity.Fixtures
{
    public class ToChangeEventsFixture : DbContextFixture<ChangedByEntityDbContext>
    {
        public override string DatabaseNamePrefix => "ChangedByEntity";

        public override bool MigrateDatabase => true;

        public ToChangeEventsFixture(MsSqlContainerFixture msSqlContainerFixture) : base(msSqlContainerFixture)
        {
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddChangedByEntity(GetConnectionString());
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<EntityCurrentUserProvider>();
            provider.CurrentUser.AsyncValue = ChangedByEntityUser.SystemUser;
        }
    }
}