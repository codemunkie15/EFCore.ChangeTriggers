namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    /// <summary>
    /// This is an internal API and should not be used in application code.
    /// </summary>
    /// <remarks>
    /// <see cref="IChange{TTracked, TChangeId}"/> is the correct interface to implement for change entities.
    /// </remarks>
    public interface IHasTrackedEntity<TTracked>
    {
        /// <summary>
        /// Gets or sets the source tracked entity
        /// </summary>
        TTracked TrackedEntity { get; set; }
    }
}
