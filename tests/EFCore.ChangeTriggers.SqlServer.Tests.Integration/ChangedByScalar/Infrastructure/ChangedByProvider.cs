using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;

internal class ChangedByProvider : ChangedByProvider<string>
{
    private readonly CurrentUserProvider currentUserProvider;

    public ChangedByProvider(CurrentUserProvider currentUserProvider)
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
