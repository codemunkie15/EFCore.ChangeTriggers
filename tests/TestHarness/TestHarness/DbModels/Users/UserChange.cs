﻿using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Models;
using System;
using TestHarness;

namespace TestHarness.DbModels.Users
{
    public class UserChange : UserBase, IChange<User, int>, IHasChangeSource<ChangeSourceType>, IHasChangedBy<User>
    {
        public int ChangeId { get; set; }

        public OperationType OperationType { get; set; }

        public ChangeSourceType ChangeSource { get; set; }

        public DateTimeOffset ChangedAt { get; set; }

        public User ChangedBy { get; set; }

        public User TrackedEntity { get; set; }
    }
}