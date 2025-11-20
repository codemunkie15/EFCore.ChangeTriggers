using EFCore.ChangeTriggers.Abstractions;

namespace TestHarness
{
    internal class ChangeSourceProvider : ChangeSourceProvider<ChangeSourceType>
    {
        public override ChangeSourceType GetChangeSource()
        {
            return ChangeSourceType.ConsoleApp;
        }

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
