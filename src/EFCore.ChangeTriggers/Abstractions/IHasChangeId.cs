namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// This is an internal API and should not be used in application code.
    /// </summary>
    /// <remarks>
    /// <see cref="IChange{TTracked, TChangeId}"/> is the correct interface to implement for change entities.
    /// </remarks>
    public interface IHasChangeId<TChangeId>
    {
        /// <summary>
        /// Gets or sets the change identifier
        /// </summary>
        TChangeId ChangeId { get; set; }
    }
}
