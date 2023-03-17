using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Threading.Tasks;

namespace _01_FullyFeatured
{
    internal class ChangeSourceProvider : IChangeSourceProvider<int>
    {
        public Task<int> GetSourceTypeAsync()
        {
            return Task.FromResult((int)ChangeSourceType.ConsoleApp);
        }
    }
}
