using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;

public class ChangeSourceEntityProvider : ChangeSourceProvider<ChangeSource>
{
    public bool UseCustomGetMigrationChangeSource { get; set; }

    private readonly EntityChangeSourceProvider changeSourceProvider;

    public ChangeSourceEntityProvider(EntityChangeSourceProvider changeSourceProvider)
    {
        this.changeSourceProvider = changeSourceProvider;
    }

    public override Task<ChangeSource> GetChangeSourceAsync()
    {
        return Task.FromResult(changeSourceProvider.CurrentChangeSourceAsync);
    }

    public override ChangeSource GetChangeSource()
    {
        return changeSourceProvider.CurrentChangeSource;
    }

    public override ChangeSource GetMigrationChangeSource()
    {
        if (UseCustomGetMigrationChangeSource)
        {
            return changeSourceProvider.MigrationChangeSource;
        }

        return base.GetMigrationChangeSource();
    }

    public override Task<ChangeSource> GetMigrationChangeSourceAsync()
    {
        if (UseCustomGetMigrationChangeSource)
        {
            return Task.FromResult(changeSourceProvider.MigrationChangeSourceAsync);
        }

        return base.GetMigrationChangeSourceAsync();
    }
}
