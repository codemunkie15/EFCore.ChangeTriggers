using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

internal class ChangedByEntityProvider : ChangedByProvider<ChangedByEntityUser>
{
    private readonly ChangedByEntityCurrentUserProvider currentUserProvider;

    public ChangedByEntityProvider(ChangedByEntityCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<ChangedByEntityUser> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUser);
    }

    public override ChangedByEntityUser GetChangedBy()
    {
        return currentUserProvider.CurrentUser;
    }
}
