﻿using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity
{
    public class ChangedByEntityFixture : ContainerFixture<ChangedByEntityDbContext>
    {
        public override bool MigrateDatabase => true;

        protected override IServiceProvider BuildServiceProvider(string connectionString)
        {
            return ChangedByEntityServiceProviderBuilder.Build(connectionString);
        }

        protected override void SetMigrationChangeContext()
        {
            var provider = Services.GetRequiredService<EntityCurrentUserProvider>();
            provider.CurrentUserAsync = new ChangedByEntityUser { Id = 0 };
        }
    }
}
