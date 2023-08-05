using EFCore.ChangeTriggers.Abstractions;
using System.Collections.Generic;

namespace _01_FullyFeatured.DbModels.Orders
{
    public class Order : OrderBase, ITracked<OrderChange>
    {
        public ICollection<OrderChange> Changes { get; set; }
    }
}
