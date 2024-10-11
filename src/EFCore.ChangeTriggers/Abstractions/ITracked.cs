namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// Represents an entity that is configured with change triggers.
    /// </summary>
    /// <typeparam name="TChangeType">The <see cref="IChange{TTracked}"/> entity type that tracks changes for this entity.</typeparam>
    public interface ITracked<TChangeType>
    {
        /// <summary>
        /// Gets or sets the collection of changes.
        /// </summary>
        ICollection<TChangeType> Changes { get; set; }
    }
}
