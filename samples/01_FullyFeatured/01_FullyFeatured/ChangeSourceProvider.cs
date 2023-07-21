using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Threading.Tasks;

namespace _01_FullyFeatured
{
    internal class ChangeSourceProvider : IChangeSourceProvider<ChangeSourceType>
    {
        public Task<ChangeSourceType> GetChangeSourceAsync()
        {
            return Task.FromResult(ChangeSourceType.ConsoleApp);
        }
    }
}
