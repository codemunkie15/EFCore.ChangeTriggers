using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;

public class ChangedByScalarProvider : ChangedByProvider<string>
{
    private readonly ScalarCurrentUserProvider currentUserProvider;

    public ChangedByScalarProvider(ScalarCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<string> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUser.AsyncValue);
    }

    public override string GetChangedBy()
    {
        return currentUserProvider.CurrentUser.SyncValue;
    }

    public override string GetMigrationChangedBy()
    {
        return currentUserProvider.MigrationCurrentUser.SyncValue ?? base.GetMigrationChangedBy();
    }

    public override Task<string> GetMigrationChangedByAsync()
    {
        if (currentUserProvider.MigrationCurrentUser.AsyncValue is not null)
        {
            return Task.FromResult(currentUserProvider.MigrationCurrentUser.AsyncValue);
        }

        return base.GetMigrationChangedByAsync();
    }
}
