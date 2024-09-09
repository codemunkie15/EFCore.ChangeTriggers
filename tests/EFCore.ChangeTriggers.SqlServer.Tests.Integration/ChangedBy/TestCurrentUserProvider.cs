using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy.Persistence;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy;

internal class TestCurrentUserProvider
{
    public TestUser CurrentUser { get; set; }

    public TestCurrentUserProvider(TestUser currentUser)
    {
        this.CurrentUser = currentUser;
    }
}
