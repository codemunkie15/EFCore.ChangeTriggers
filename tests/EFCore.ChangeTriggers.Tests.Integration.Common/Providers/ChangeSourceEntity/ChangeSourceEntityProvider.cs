using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;

public class ChangeSourceEntityProvider : ChangeSourceProvider<ChangeSource>
{
    private readonly EntityChangeSourceProvider changeSourceProvider;

    public ChangeSourceEntityProvider(EntityChangeSourceProvider changeSourceProvider)
    {
        this.changeSourceProvider = changeSourceProvider;
    }

    public override Task<ChangeSource> GetChangeSourceAsync()
    {
        return Task.FromResult(changeSourceProvider.CurrentChangeSource.AsyncValue);
    }

    public override ChangeSource GetChangeSource()
    {
        return changeSourceProvider.CurrentChangeSource.SyncValue;
    }

    public override ChangeSource GetMigrationChangeSource()
    {
        return changeSourceProvider.MigrationChangeSource.SyncValue ?? base.GetMigrationChangeSource();
    }

    public override Task<ChangeSource> GetMigrationChangeSourceAsync()
    {
        if (changeSourceProvider.MigrationChangeSource.AsyncValue is not null)
        {
            return Task.FromResult(changeSourceProvider.MigrationChangeSource.AsyncValue);
        }

        return base.GetMigrationChangeSourceAsync();
    }
}
