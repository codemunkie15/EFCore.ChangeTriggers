using EFCore.ChangeTriggers.Abstractions;
using System.Collections.Generic;
using TestHarness.DbModels.PaymentMethods;
using TestHarness.DbModels.Permissions;

namespace TestHarness.DbModels.Users
{
    public class User : UserBase, ITracked<UserChange>
    {
        public User()
        {
            Permissions = new List<Permission>();
            PaymentMethods = new List<PaymentMethod>();
        }

        public ICollection<Permission> Permissions { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public ICollection<UserChange> Changes { get; set; }
    }
}