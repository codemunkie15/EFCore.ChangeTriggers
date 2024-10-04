using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.ChangeTriggers.Infrastructure
{
    public abstract class ChangeTriggersDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        private new ChangeTriggersDbContextOptionsExtension Extension
            => (ChangeTriggersDbContextOptionsExtension)base.Extension;

        public ChangeTriggersDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is ChangeTriggersDbContextOptionsExtensionInfo otherInfo &&
                Extension.TriggerNameFactory == otherInfo.Extension.TriggerNameFactory &&
                Extension.ChangedByTypes == otherInfo.Extension.ChangedByTypes &&
                Extension.ChangeSourceTypes == otherInfo.Extension.ChangeSourceTypes;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool IsDatabaseProvider => false;
    }
}
