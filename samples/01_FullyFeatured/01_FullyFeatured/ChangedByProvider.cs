using _01_FullyFeatured.DbModels.Users;
using EFCore.ChangeTriggers.Abstractions;
using System.Threading.Tasks;

namespace _01_FullyFeatured
{
    internal class ChangedByProvider : IChangedByProvider<User>
    {
        public Task<User> GetChangedByAsync()
        {
            return Task.FromResult(new User { Id = 1 });
        }
    }
}
