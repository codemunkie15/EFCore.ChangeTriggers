using EFCore.ChangeTriggers.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.MySql.Infrastructure
{
    /// <summary>
    /// Allows SQL Server specific configuration to be performed on <see cref="DbContextOptions" />.
    /// </summary>
    public class ChangeTriggersMySqlDbContextOptionsBuilder
        : ChangeTriggersDbContextOptionsBuilder<ChangeTriggersMySqlDbContextOptionsBuilder, ChangeTriggersMySqlDbContextOptionsExtension>
    {
        public ChangeTriggersMySqlDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
            : base(optionsBuilder)
        {
        }
    }
}
