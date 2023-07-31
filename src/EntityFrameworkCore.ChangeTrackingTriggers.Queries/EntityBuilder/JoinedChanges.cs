namespace EntityFrameworkCore.ChangeTrackingTriggers.Queries.EntityBuilder
{
    public class JoinedChanges<TChange>
    {
        public TChange ChangeEntity { get; set; }

        public TChange PreviousChangeEntity { get; set; }
    }
}
