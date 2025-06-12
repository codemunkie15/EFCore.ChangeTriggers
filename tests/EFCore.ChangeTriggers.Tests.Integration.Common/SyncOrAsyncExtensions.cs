using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.ChangeTriggers.Tests.Integration.Common
{
    public static class SyncOrAsyncExtensions
    {
        public static async Task SaveChangesSyncOrAsync(this DbContext dbContext, bool useAsync)
        {
            if (useAsync)
            {
                await dbContext.SaveChangesAsync();
            }
            else
            {
                dbContext.SaveChanges();
            }
        }

        public static async Task MigrateSyncOrAsync(this DatabaseFacade database, bool useAsync)
        {
            if (useAsync)
            {
                await database.MigrateAsync();
            }
            else
            {
                database.Migrate();
            }
        }
    }
}
