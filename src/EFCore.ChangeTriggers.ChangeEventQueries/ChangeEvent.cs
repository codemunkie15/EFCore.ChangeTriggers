namespace EFCore.ChangeTriggers.ChangeEventQueries
{
    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent
    {
        /// <summary>
        /// The description of the change event
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Gets or sets the old/previous value.
        /// </summary>
        public string? OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new/next value.
        /// </summary>
        public string? NewValue { get; set; }

        /// <summary>
        /// Gets or sets when the change occured.
        /// </summary>
        public DateTimeOffset ChangedAt { get; set; }
    }

    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent<TChangedBy, TChangeSource> : ChangeEvent
    {
        /// <summary>
        /// Gets or sets who made the change.
        /// </summary>
        public TChangedBy? ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets where the change originated from.
        /// </summary>
        public TChangeSource? ChangeSource { get; set; }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangedByEvents
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent<TChangedBy> : ChangeEvent
    {
        /// <summary>
        /// Gets or sets who made the change.
        /// </summary>
        public TChangedBy? ChangedBy { get; set; }
    }
}

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EFCore.ChangeTriggers.ChangeEventQueries.ChangeSourceEvents
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a data change to a property.
    /// </summary>
    public class ChangeEvent<TChangeSource> : ChangeEvent
    {
        /// <summary>
        /// Gets or sets where the change originated from.
        /// </summary>
        public TChangeSource? ChangeSource { get; set; }
    }
}