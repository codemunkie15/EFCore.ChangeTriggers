namespace EntityFrameworkCore.ChangeTrackingTriggers.ChangeEventQueries
{
    public class ChangeEvent
    {
        public string Description { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTimeOffset ChangedAt { get; set; }
    }

    public class ChangeEvent<TChangedBy, TChangeSource> : ChangeEvent
    {
        public TChangedBy? ChangedBy { get; set; }

        public TChangeSource? ChangeSource { get; set; }
    }

    public class ChangeSourceChangeEvent<TChangeSource> : ChangeEvent
    {
        public TChangeSource? ChangeSource { get; set; }
    }

    public class ChangedByChangeEvent<TChangedBy> : ChangeEvent
    {
        public TChangedBy? ChangedBy { get; set; }
    }
}
