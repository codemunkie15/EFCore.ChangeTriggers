using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Infrastructure;

internal class ChangeSourceScalarProvider : ChangeSourceProvider<ChangeSource>
{
    private readonly ScalarChangeSourceProvider changeSourceProvider;

    public ChangeSourceScalarProvider(ScalarChangeSourceProvider changeSourceProvider)
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
