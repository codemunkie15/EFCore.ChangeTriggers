﻿using TestHarness.DbModels.PaymentMethods;

namespace TestHarness.DbModels.Users
{
    public abstract class UserBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DateOfBirth { get; set; }

        public PaymentMethod? PrimaryPaymentMethod { get; set; }
    }
}
