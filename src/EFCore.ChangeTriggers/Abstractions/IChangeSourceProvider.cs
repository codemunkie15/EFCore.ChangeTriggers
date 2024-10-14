namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// Represents a service that provides the source of changes.
    /// </summary>
    /// <typeparam name="TChangeSource">The change source type (either a static value e.g. an enum integer, or an EF Core entity).</typeparam>
    public interface IChangeSourceProvider<TChangeSource>
    {
        /// <summary>
        /// Gets the value of the source of changes. Implement this method if you are using EF Core synchronously.
        /// </summary>
        /// <returns>Either a static value e.g. an enum, or an EF Core entity.</returns>
        TChangeSource GetChangeSource();

        /// <summary>
        /// Gets the value of the source of changes. Implement this method if you are using EF Core asynchronously.
        /// </summary>
        /// <returns>Either a static value e.g. an enum, or an EF Core entity.</returns>
        Task<TChangeSource> GetChangeSourceAsync();

        /// <summary>
        /// Gets the value of the source of changes when running migrations. Implement this method if you are using EF Core synchronously.
        /// </summary>
        /// <returns>Either a static value e.g. an enum, or an EF Core entity.</returns>
        TChangeSource GetMigrationChangeSource();

        /// <summary>
        /// Gets the value of the source of changes when running migrations. Implement this method if you are using EF Core asynchronously.
        /// </summary>
        /// <returns>Either a static value e.g. an enum, or the foreign key to an EF Core entity.</returns>
        Task<TChangeSource> GetMigrationChangeSourceAsync();
    }
}
