using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Scopes
{
    public sealed class ChangeSourceScalarTestScope : TestScope<ChangeSourceScalarDbContext, ChangeSourceScalarUser, ChangeSourceScalarUserChange>
    {
        public ScalarChangeSourceProvider ChangeSourceProvider { get; }

        public ChangeSourceScalarTestScope(IServiceProvider services)
            : base(services)
        {
            ChangeSourceProvider = scope.ServiceProvider.GetRequiredService<ScalarChangeSourceProvider>();
        }
    }
}
