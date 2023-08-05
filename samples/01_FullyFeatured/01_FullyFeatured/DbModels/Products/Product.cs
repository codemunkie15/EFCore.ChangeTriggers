using _01_FullyFeatured.DbModels.Orders;
using EFCore.ChangeTriggers.Abstractions;
using System.Collections.Generic;

namespace _01_FullyFeatured.DbModels.Products
{
    public class Product : ProductBase, ITracked<ProductChange>
    {
        public ICollection<Order> Orders { get; set; }

        public ICollection<ProductChange> Changes { get; set; }
    }
}
