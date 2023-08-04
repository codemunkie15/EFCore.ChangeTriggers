namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder
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
        public TChange ChangeEntity { get; set; }

        /// <summary>
        /// The previous change entity.
        /// </summary>
        public TChange PreviousChangeEntity { get; set; }
    }
}
