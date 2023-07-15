using EntityFrameworkCore.ChangeTrackingTriggers.Models;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    /// <summary>
    /// Represents a change entity that tracks the changes of a <see cref="TTracked"/> entity.
    /// </summary>
    /// <typeparam name="TTracked">The entity type that this change entity tracks.</typeparam>
    /// <typeparam name="TChangeIdType">The primary key type of the entity.</typeparam>
    public interface IChange<TTracked, TChangeIdType>
    {
        /// <summary>
        /// Gets or sets the change identifier
        /// </summary>
        TChangeIdType ChangeId { get; set; }

        /// <summary>
        /// Gets or sets the change operation type
        /// </summary>
        OperationType OperationType { get; set; }

        /// <summary>
        /// Gets or sets when the change occured
        /// </summary>
        DateTimeOffset ChangedAt { get; set; }

        /// <summary>
        /// Gets or sets the source tracked entity
        /// </summary>
        TTracked TrackedEntity { get; set; }
    }
}
