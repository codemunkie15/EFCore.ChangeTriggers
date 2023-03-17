using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using System.Collections.Generic;

namespace _01_FullyFeatured.DbModels.Users
{
    public class User : UserBase, ITracked<UserChange>
    {
        public ICollection<UserChange> Changes { get; set; }
    }
}