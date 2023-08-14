namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// Represents a service that provides who changes are made by.
    /// </summary>
    /// <typeparam name="TChangedBy">The changed by type (either a static value e.g. a username string, or an EF Core entity).</typeparam>
    public interface IChangedByProvider<TChangedBy>
    {
        /// <summary>
        /// Gets the value of who changes are made by. Implement this method if you are using EF Core synchronously.
        /// </summary>
        /// <returns>Either a static value e.g. a username string, or an EF Core entity.</returns>
        TChangedBy GetChangedBy();

        /// <summary>
        /// Gets the value of who changes are made by. Implement this method if you are using EF Core asynchronously.
        /// </summary>
        /// <returns>Either a static value e.g. a username string, or an EF Core entity.</returns>
        Task<TChangedBy> GetChangedByAsync();

        /// <summary>
        /// Gets the value of who changes are made by when running migrations. Implement this method if you are using EF Core synchronously.
        /// </summary>
        /// <returns>Either a static value e.g. a username string, or an EF Core entity.</returns>
        TChangedBy GetMigrationChangedBy();

        /// <summary>
        /// Gets the value of who changes are made by when running migrations. Implement this method if you are using EF Core asynchronously.
        /// </summary>
        /// <returns>Either a static value e.g. a username string, or an EF Core entity.</returns>
        Task<TChangedBy> GetMigrationChangedByAsync();
    }
}
