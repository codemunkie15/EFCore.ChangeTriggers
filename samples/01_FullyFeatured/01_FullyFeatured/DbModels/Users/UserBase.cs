﻿using System;

namespace _01_FullyFeatured.DbModels.Users
{
    public abstract class UserBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DateOfBirth { get; set; }
    }
}
