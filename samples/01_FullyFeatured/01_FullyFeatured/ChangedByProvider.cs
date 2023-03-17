using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Threading.Tasks;

namespace _01_FullyFeatured
{
    internal class ChangedByProvider : IChangedByProvider<int>
    {
        public Task<int> GetChangedById()
        {
            return Task.FromResult(1);
        }
    }
}
