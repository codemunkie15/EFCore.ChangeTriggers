using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Models
{
    internal class ChangeTrackedEntity
    {
        public IEntityType TrackedEntityType { get; set; }

        public IEntityType ChangeEntityType { get; set; }

        public ChangeTrigger Trigger { get; set; }
    }
}
