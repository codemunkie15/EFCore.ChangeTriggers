using TestHarness.DbModels.Users;

namespace TestHarness
{
    internal class CurrentUserProvider
    {
        private readonly User currentUser;

        public CurrentUserProvider(User currentUser)
        {
            this.currentUser = currentUser;
        }

        public User GetCurrentUser() => currentUser;
    }
}
