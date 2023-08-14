using EFCore.ChangeTriggers.Models;

namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// This is an internal API and should not be used in application code.
    /// </summary>
    /// <remarks>
    /// <see cref="IChange{TTracked, TChangeId}"/> is the correct interface to implement for change entities.
    /// </remarks>
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
