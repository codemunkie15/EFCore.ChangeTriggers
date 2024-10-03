using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

internal class ChangedByProvider : ChangedByProvider<EntityUser>
{
    private readonly CurrentUserProvider currentUserProvider;

    public ChangedByProvider(CurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<EntityUser> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUser);
    }

    public override EntityUser GetChangedBy()
    {
        return currentUserProvider.CurrentUser;
    }
}
