using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    internal class TestChangedByProvider : ChangedByProvider<TestUser>
    {
        private readonly TestCurrentUserProvider currentUserProvider;

        public TestChangedByProvider(TestCurrentUserProvider currentUserProvider)
        {
            this.currentUserProvider = currentUserProvider;
        }

        public override Task<TestUser> GetChangedByAsync()
        {
            return Task.FromResult(currentUserProvider.GetCurrentUser());
        }
    }
}
