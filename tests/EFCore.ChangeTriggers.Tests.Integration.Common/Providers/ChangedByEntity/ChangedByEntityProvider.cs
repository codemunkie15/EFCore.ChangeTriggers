using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;

public class ChangedByEntityProvider : ChangedByProvider<ChangedByEntityUser>
{
    private readonly EntityCurrentUserProvider currentUserProvider;

    public ChangedByEntityProvider(EntityCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<ChangedByEntityUser> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUser.AsyncValue);
    }

    public override ChangedByEntityUser GetChangedBy()
    {
        return currentUserProvider.CurrentUser.SyncValue;
    }

    public override ChangedByEntityUser GetMigrationChangedBy()
    {
        return currentUserProvider.MigrationCurrentUser.SyncValue ?? base.GetMigrationChangedBy();
    }

    public override Task<ChangedByEntityUser> GetMigrationChangedByAsync()
    {
        if (currentUserProvider.MigrationCurrentUser.AsyncValue is not null)
        {
            return Task.FromResult(currentUserProvider.MigrationCurrentUser.AsyncValue);
        }

        return base.GetMigrationChangedByAsync();
    }
}
