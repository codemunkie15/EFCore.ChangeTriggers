namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// Represents a change entity with the ability to track where the change originated from.
    /// </summary>
    /// <typeparam name="TChangeSource">The change source type (either a CLI type e.g. an enum, or an EF Core entity).</typeparam>
    public interface IHasChangeSource<TChangeSource>
    {
        /// <summary>
        /// Gets or sets where the change originated from.
        /// </summary>
        TChangeSource ChangeSource { get; set; }
    }
}
