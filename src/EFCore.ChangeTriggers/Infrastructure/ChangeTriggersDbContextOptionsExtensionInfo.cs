using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.ChangeTriggers.Infrastructure
{
    public abstract class ChangeTriggersDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ChangeTriggersDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override int GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool IsDatabaseProvider => false;
    }
}
