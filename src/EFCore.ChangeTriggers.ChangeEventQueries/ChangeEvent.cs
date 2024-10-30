namespace EFCore.ChangeTriggers.ChangeEventQueries
{
    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent
    {
        public required string Description { get; set; }

        public string? OldValue { get; set; }

        public string? NewValue { get; set; }

        public DateTimeOffset ChangedAt { get; set; }
    }

    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent<TChangedBy, TChangeSource> : ChangeEvent
    {
        public TChangedBy? ChangedBy { get; set; }

        public TChangeSource? ChangeSource { get; set; }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent<TChangedBy> : ChangeEvent
    {
        public TChangedBy? ChangedBy { get; set; }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent<TChangeSource> : ChangeEvent
    {
        public TChangeSource? ChangeSource { get; set; }
    }
}