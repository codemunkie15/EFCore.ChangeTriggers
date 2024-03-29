﻿using _01_FullyFeatured.DbModels.Orders;
using EFCore.ChangeTriggers.Abstractions;
using System.Collections.Generic;

namespace _01_FullyFeatured.DbModels.Users
{
    public class User : UserBase, ITracked<UserChange>
    {
        public ICollection<Order> Orders { get; set; }

        public ICollection<UserChange> Changes { get; set; }
    }
}