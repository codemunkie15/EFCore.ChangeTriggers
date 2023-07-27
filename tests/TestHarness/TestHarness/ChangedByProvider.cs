using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Threading.Tasks;
using TestHarness.DbModels.Users;

namespace TestHarness
{
    internal class ChangedByProvider : IChangedByProvider<User>
    {
        public Task<User> GetChangedByAsync()
        {
            return Task.FromResult(new User { Id = 1 });
        }
    }
}
