using EFCore.ChangeTriggers.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Infrastructure
{
    /// <summary>
    /// Allows SQL Server specific configuration to be performed on <see cref="DbContextOptions" />.
    /// </summary>
    public class ChangeTriggersSqlServerDbContextOptionsBuilder
        : ChangeTriggersDbContextOptionsBuilder<ChangeTriggersSqlServerDbContextOptionsBuilder, ChangeTriggersSqlServerDbContextOptionsExtension>
    {
        private readonly DbContextOptionsBuilder optionsBuilder;

        public ChangeTriggersSqlServerDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
            : base(optionsBuilder)
        {
            this.optionsBuilder = optionsBuilder;
        }
    }
}
