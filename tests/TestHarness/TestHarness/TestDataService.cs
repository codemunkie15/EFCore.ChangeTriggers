using System.Threading.Tasks;
using TestHarness.DbModels.PaymentMethods;
using TestHarness.DbModels.Users;

namespace TestHarness
{
    internal class TestDataService
    {
        private readonly MyDbContext dbContext;

        public TestDataService(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync()
        {
            var user = new User
            {
                Name = "Some user",
                DateOfBirth = "01/01/2000"
            };
            dbContext.Users.Add(user);

            var pm = new PaymentMethod
            {
                Name = "Payment method 1"
            };

            user.PaymentMethods.Add(pm);

            await dbContext.SaveChangesAsync();

            user.PrimaryPaymentMethod = pm;

            await dbContext.SaveChangesAsync();
        }
    }
}
