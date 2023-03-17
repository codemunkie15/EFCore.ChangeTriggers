using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Models
{
    internal class ChangeTrackedEntity
    {
        public IEntityType TrackedEntityType { get; set; }

        public IEntityType ChangeEntityType { get; set; }

        public ChangeTrackingTrigger Trigger { get; set; }
    }
}
