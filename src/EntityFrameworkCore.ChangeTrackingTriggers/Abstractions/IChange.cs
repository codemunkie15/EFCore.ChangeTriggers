using EntityFrameworkCore.ChangeTrackingTriggers.Models;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface IChange<TTracked, TChangeIdType>
    {
        TChangeIdType ChangeId { get; set; }

        OperationType OperationType { get; set; }

        DateTimeOffset ChangedAt { get; set; }

        TTracked TrackedEntity { get; set; }
    }
}
