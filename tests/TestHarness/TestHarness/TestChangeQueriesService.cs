using EFCore.ChangeTriggers.ChangeEventQueries;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using Microsoft.EntityFrameworkCore;
using TestHarness.DbModels.Users;

namespace TestHarness
{
    internal class TestChangeQueriesService
    {
        private readonly MyDbContext dbContext;

        public TestChangeQueriesService(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task RunAsync()
        {
            var configuration = new ChangeEventQueryConfiguration()
                .ForEntity<UserChange>(builder =>
                {
                    builder.AddProperty(uc => uc.Name);
                    builder.AddProperty(uc => uc.PrimaryPaymentMethod.Name);
                });

            var query = await dbContext.UserChanges.ToChangeEvents<User, ChangeSourceType>(configuration).ToListAsync();
        }
    }
}
