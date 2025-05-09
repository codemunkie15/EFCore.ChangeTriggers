using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;

public class ChangedByEntityProvider : ChangedByProvider<ChangedByEntityUser>
{
    public bool UseCustomGetMigrationChangedBy { get; set; }

    private readonly EntityCurrentUserProvider currentUserProvider;

    public ChangedByEntityProvider(EntityCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<ChangedByEntityUser> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUserAsync);
    }

    public override ChangedByEntityUser GetChangedBy()
    {
        return currentUserProvider.CurrentUser;
    }

    public override ChangedByEntityUser GetMigrationChangedBy()
    {
        if (UseCustomGetMigrationChangedBy)
        {
            return currentUserProvider.MigrationCurrentUser;
        }

        return base.GetMigrationChangedBy();
    }

    public override Task<ChangedByEntityUser> GetMigrationChangedByAsync()
    {
        if (UseCustomGetMigrationChangedBy)
        {
            return Task.FromResult(currentUserProvider.MigrationCurrentUserAsync);
        }

        return base.GetMigrationChangedByAsync();
    }
}
