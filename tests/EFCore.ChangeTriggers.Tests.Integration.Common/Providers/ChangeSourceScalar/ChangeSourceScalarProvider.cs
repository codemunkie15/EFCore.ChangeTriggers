using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;

public class ChangeSourceScalarProvider : ChangeSourceProvider<ChangeSourceType>
{
    public bool UseCustomGetMigrationChangeSource { get; set; }

    private readonly ScalarChangeSourceProvider changeSourceProvider;

    public ChangeSourceScalarProvider(ScalarChangeSourceProvider changeSourceProvider)
    {
        this.changeSourceProvider = changeSourceProvider;
    }

    public override Task<ChangeSourceType> GetChangeSourceAsync()
    {
        return Task.FromResult(changeSourceProvider.CurrentChangeSource.AsyncValue);
    }

    public override ChangeSourceType GetChangeSource()
    {
        return changeSourceProvider.CurrentChangeSource.SyncValue;
    }

    public override ChangeSourceType GetMigrationChangeSource()
    {
        if (UseCustomGetMigrationChangeSource)
        {
            return changeSourceProvider.MigrationChangeSource.SyncValue;
        }

        return base.GetMigrationChangeSource();
    }

    public override Task<ChangeSourceType> GetMigrationChangeSourceAsync()
    {
        if (UseCustomGetMigrationChangeSource)
        {
            return Task.FromResult(changeSourceProvider.MigrationChangeSource.AsyncValue);
        }

        return base.GetMigrationChangeSourceAsync();
    }
}
