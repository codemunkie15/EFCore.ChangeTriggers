using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure;

internal class MigrationsChangedByProvider : ChangedByProvider<MigrationsUser>
{
    private readonly MigrationsCurrentUserProvider currentUserProvider;

    public MigrationsChangedByProvider(MigrationsCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<MigrationsUser> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUser);
    }

    public override MigrationsUser GetChangedBy()
    {
        return currentUserProvider.CurrentUser;
    }
}
