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

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return string.Equals(LogFragment, other.LogFragment, StringComparison.Ordinal);
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool IsDatabaseProvider => false;
    }
}
