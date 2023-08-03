using TestHarness.DbModels.Users;

namespace TestHarness.DbModels.PaymentMethods
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public User User { get; set; }
    }
}
