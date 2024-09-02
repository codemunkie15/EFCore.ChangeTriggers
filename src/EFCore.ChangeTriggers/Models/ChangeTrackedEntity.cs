﻿using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Models
{
    internal class ChangeTrackedEntity
    {
        public required IEntityType TrackedEntityType { get; set; }

        public required IEntityType ChangeEntityType { get; set; }

        public required ChangeTrigger Trigger { get; set; }
    }
}
