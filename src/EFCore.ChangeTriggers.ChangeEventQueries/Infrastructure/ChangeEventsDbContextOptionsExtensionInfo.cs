using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure
{
    public class ChangeEventsDbContextOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {
        private new ChangeEventsDbContextOptionsExtension Extension
            => (ChangeEventsDbContextOptionsExtension)base.Extension;

        public ChangeEventsDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override int GetServiceProviderHashCode() => 0;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is ChangeEventsDbContextOptionsExtensionInfo;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
        }

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "EFCore.ChangeTriggers.ChangeEventQueries";
    }
}
