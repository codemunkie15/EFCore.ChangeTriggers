using EFCore.ChangeTriggers.Abstractions;
using System.Threading.Tasks;

namespace TestHarness
{
    internal class ChangeSourceProvider : ChangeSourceProvider<ChangeSourceType>
    {
        public override Task<ChangeSourceType> GetChangeSourceAsync()
        {
            return Task.FromResult(ChangeSourceType.ConsoleApp);
        }

        public override ChangeSourceType GetMigrationChangeSource()
        {
            return ChangeSourceType.Migration;
        }
    }
}
