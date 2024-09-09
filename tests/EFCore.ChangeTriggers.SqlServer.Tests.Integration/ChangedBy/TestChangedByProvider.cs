using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy.Persistence;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy;

internal class TestChangedByProvider : ChangedByProvider<TestUser>
{
    private readonly TestCurrentUserProvider currentUserProvider;

    public TestChangedByProvider(TestCurrentUserProvider currentUserProvider)
    {
        this.currentUserProvider = currentUserProvider;
    }

    public override Task<TestUser> GetChangedByAsync()
    {
        return Task.FromResult(currentUserProvider.CurrentUser);
    }

    public override TestUser GetChangedBy()
    {
        return currentUserProvider.CurrentUser;
    }
}
