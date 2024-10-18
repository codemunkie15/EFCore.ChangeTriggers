using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Infrastructure;

internal class ChangeSourceEntityProvider : ChangeSourceProvider<ChangeSource>
{
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
}
