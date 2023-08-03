namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries.EntityBuilder
{
    public class JoinedChanges<TChange>
    {
        public TChange ChangeEntity { get; set; }

        public TChange PreviousChangeEntity { get; set; }
    }
}
