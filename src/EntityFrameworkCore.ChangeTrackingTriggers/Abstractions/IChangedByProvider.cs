namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    /// <summary>
    /// Represents a service that provides who changes are made by.
    /// </summary>
    /// <typeparam name="TChangedBy">The changed by type (either a static value e.g. a username string, or an EF Core entity).</typeparam>
    public interface IChangedByProvider<TChangedBy>
    {
        /// <summary>
        /// Gets the value of who changes are made by.
        /// </summary>
        /// <returns>Either a static value e.g. a username string, or an EF Core entity.</returns>
        Task<TChangedBy> GetChangedByAsync();
    }
}
