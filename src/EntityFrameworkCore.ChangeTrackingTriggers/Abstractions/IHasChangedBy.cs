namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    /// <summary>
    /// Represents a change entity with the ability to track who the change was made by.
    /// </summary>
    /// <typeparam name="TChangedBy">The changed by type (either a CLI type e.g. a string, or an EF Core entity).</typeparam>
    public interface IHasChangedBy<TChangedBy>
    {
        /// <summary>
        /// Gets or sets who made the change.
        /// </summary>
        TChangedBy ChangedBy { get; set; }
    }
}
