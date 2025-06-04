using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Scopes
{
    public sealed class ChangedByScalarTestScope : TestScope<ChangedByScalarDbContext>
    {
        public ScalarCurrentUserProvider CurrentUserProvider { get; }

        public ChangedByScalarTestScope(IServiceProvider services)
            : base(services)
        {
            CurrentUserProvider = scope.ServiceProvider.GetRequiredService<ScalarCurrentUserProvider>();
        }
    }
}
