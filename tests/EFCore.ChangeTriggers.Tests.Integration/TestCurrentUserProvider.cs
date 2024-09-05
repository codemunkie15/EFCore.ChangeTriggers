namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    internal class TestCurrentUserProvider
    {
        private readonly TestUser currentUser;

        public TestCurrentUserProvider(TestUser currentUser)
        {
            this.currentUser = currentUser;
        }

        public TestUser GetCurrentUser() => currentUser;
    }
}
