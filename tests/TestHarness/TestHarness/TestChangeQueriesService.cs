using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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
            var query = dbContext
                .CreateChangeEventQueryBuilder<User, ChangeSourceType>()
                .AddChanges(
                    dbContext.UserChanges.Where(uc => uc.TrackedEntity.Id == 1),
                    builder =>
                    {
                        builder
                            .AddEntityProperties(prop => $"{prop} updated!")
                            .AddProperty("Primary payment method changed", e => e.PrimaryPaymentMethod.Name);
                    }
                ).Build();

                var query2 = dbContext
                    .CreateChangeEventQueryBuilder()
                    .AddChanges(
                        dbContext.PermissionChanges,
                        builder =>
                        {
                            builder
                                .AddProperty("Name changed", e => e.Name)
                                .AddProperty("Order changed", e => e.Order.ToString())
                                .AddProperty("Reference changed", e => e.Reference.ToString())
                                .AddProperty("Enabled changed", e => e.Enabled.ToString());
                        })
                    .Build();

                var changes1 = query.OrderBy(ce => ce.ChangedAt).ToListAsync();
                var changes2 = await query2.OrderBy(ce => ce.ChangedAt).ToListAsync();
        }
    }
}
