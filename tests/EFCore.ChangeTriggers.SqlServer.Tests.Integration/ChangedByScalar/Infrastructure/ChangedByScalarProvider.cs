using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;

internal class ChangedByScalarProvider : ChangedByProvider<string>
{
    private readonly ChangedByScalarCurrentUserProvider currentUserProvider;

    public ChangedByScalarProvider(ChangedByScalarCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<string> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUser);
    }

    public override string GetChangedBy()
    {
        return currentUserProvider.CurrentUser;
    }
}
