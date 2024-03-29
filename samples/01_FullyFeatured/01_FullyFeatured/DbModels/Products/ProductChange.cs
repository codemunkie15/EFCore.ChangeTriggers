﻿using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;
using System;

namespace _01_FullyFeatured.DbModels.Products
{
    public class ProductChange : ProductBase, IChange<Product, int>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public Product TrackedEntity { get; set; }
    }
}
