using EFCore.ChangeTriggers.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.ChangeTriggers.MySql.Infrastructure
{
    public class ChangeTriggersMySqlDbContextOptionsExtensionInfo : ChangeTriggersDbContextOptionsExtensionInfo
    {
        public ChangeTriggersMySqlDbContextOptionsExtensionInfo(IDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        public override string LogFragment => "EFCore.ChangeTriggers.MySql";
    }
}