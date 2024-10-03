using EFCore.ChangeTriggers.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.ChangeTriggers.SqlServer.Infrastructure
{
    public class ChangeTriggersSqlServerDbContextOptionsExtensionInfo : ChangeTriggersDbContextOptionsExtensionInfo
    {
        public ChangeTriggersSqlServerDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override string LogFragment => "EFCore.ChangeTriggers.SqlServer";

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
        {
            return other is ChangeTriggersSqlServerDbContextOptionsExtensionInfo;
        }
    }
}