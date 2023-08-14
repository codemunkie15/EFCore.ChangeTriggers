using _01_FullyFeatured.DbModels.Products;
using _01_FullyFeatured.DbModels.Users;

namespace _01_FullyFeatured.DbModels.Orders
{
    public abstract class OrderBase
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public Product Product { get; set; }

        public User User { get; set; }
    }
}
