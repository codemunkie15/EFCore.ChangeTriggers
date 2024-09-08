using EFCore.ChangeTriggers.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace EFCore.ChangeTriggers.SqlServer.Infrastructure
{
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
