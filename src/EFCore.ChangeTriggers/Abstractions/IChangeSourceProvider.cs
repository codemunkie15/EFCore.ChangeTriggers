namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// Represents a service that provides the source of changes.
    /// </summary>
    /// <typeparam name="TChangeSource">The change source type (either a static value e.g. an enum integer, or the foreign key to an EF Core entity).</typeparam>
    public interface IChangeSourceProvider<TChangeSource>
    {
        /// <summary>
        /// Gets the value of the source of changes.
        /// </summary>
        /// <returns>Either a static value e.g. an enum integer, or the foreign key to an EF Core entity.</returns>
        Task<TChangeSource> GetChangeSourceAsync();
    }
}
