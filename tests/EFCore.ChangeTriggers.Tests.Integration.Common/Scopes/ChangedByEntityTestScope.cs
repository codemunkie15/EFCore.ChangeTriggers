using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Scopes
{
    public sealed class ChangedByEntityTestScope : TestScope<ChangedByEntityDbContext>
    {
        public EntityCurrentUserProvider CurrentUserProvider { get; }

        public ChangedByEntityTestScope(IServiceProvider services)
            : base(services)
        {
            CurrentUserProvider = scope.ServiceProvider.GetRequiredService<EntityCurrentUserProvider>();
        }
    }
}
