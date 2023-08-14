namespace EFCore.ChangeTriggers.EfCoreExtension
{
    internal class ChangeTriggersExtensionContext
    {
        public bool IsMigrationRunning { get; private set; }

        public void StartMigration()
        {
            IsMigrationRunning = true;
        }

        public void EndMigration()
        {
            IsMigrationRunning = false;
        }
    }
}