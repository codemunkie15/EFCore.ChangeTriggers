namespace EFCore.ChangeTriggers.ChangeEventQueries.EntityBuilder
{
    /// <summary>
    /// Represents a join between a change entity and the previous change entity for determining old and new values.
    /// </summary>
    /// <typeparam name="TChange">The change entity type.</typeparam>
    public class JoinedChanges<TChange>
    {
        /// <summary>
        /// The current change entity.
        /// </summary>
        public required TChange ChangeEntity { get; set; }

        /// <summary>
        /// The previous change entity.
        /// </summary>
        public required TChange PreviousChangeEntity { get; set; }
    }
}
