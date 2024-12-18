using EFCore.ChangeTriggers.ChangeEventQueries;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using Microsoft.EntityFrameworkCore;
using TestHarness.DbModels.Permissions;
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
            var query = await dbContext.UserChanges.Where(uc => uc.Id == 1).ToChangeEvents<User, ChangeSourceType>(
                //new ChangeEventConfiguration(builder =>
                //{
                //    builder.Configure<UserChange>(uc =>
                //    {
                //        uc.AddProperty(uc => uc.Name);
                //        uc.AddProperty(uc => uc.PrimaryPaymentMethod.Name)
                //            .WithDescription("Payment method changed");
                //    });
                //}))
                ).OrderBy(ce => ce.ChangedAt).ToListAsync();
        }
    }
}
