using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Scopes
{
    public sealed class ChangeSourceEntityTestScope : TestScope<ChangeSourceEntityDbContext>
    {
        public EntityChangeSourceProvider ChangeSourceProvider { get; }

        public ChangeSourceEntityTestScope(IServiceProvider services)
            : base(services)
        {
            ChangeSourceProvider = scope.ServiceProvider.GetRequiredService<EntityChangeSourceProvider>();
        }
    }
}
