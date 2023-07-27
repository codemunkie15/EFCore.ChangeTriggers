using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Threading.Tasks;

namespace TestHarness
{
    internal class ChangeSourceProvider : IChangeSourceProvider<ChangeSourceType>
    {
        public Task<ChangeSourceType> GetChangeSourceAsync()
        {
            return Task.FromResult(ChangeSourceType.ConsoleApp);
        }
    }
}
