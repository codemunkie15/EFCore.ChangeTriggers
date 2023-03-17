using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EntityFrameworkCore.ChangeTrackingTriggers.EfCoreExtension
{
    internal class ChangeTrackingExtensionInfo : DbContextOptionsExtensionInfo
    {
        public ChangeTrackingExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override int GetServiceProviderHashCode()
        {
            return 0;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return string.Equals(LogFragment, other.LogFragment, StringComparison.Ordinal);
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "ChangeTrackingTriggersExtension";
    }
}
