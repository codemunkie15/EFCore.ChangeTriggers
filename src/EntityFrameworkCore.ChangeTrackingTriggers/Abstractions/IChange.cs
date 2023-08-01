using EntityFrameworkCore.ChangeTrackingTriggers.Models;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    /// <summary>
    /// Represents a change entity that tracks the changes of an entity.
    /// </summary>
    public interface IChange
    {
        /// <summary>
        /// Gets or sets the change operation type
        /// </summary>
        OperationType OperationType { get; set; }

        /// <summary>
        /// Gets or sets when the change occured
        /// </summary>
        DateTimeOffset ChangedAt { get; set; }
    }
}
