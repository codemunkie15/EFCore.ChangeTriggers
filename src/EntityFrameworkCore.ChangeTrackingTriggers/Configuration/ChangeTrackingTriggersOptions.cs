namespace EntityFrameworkCore.ChangeTrackingTriggers.Configuration
{
    /// <summary>
    /// Provides an API for configuring change tracking triggers.
    /// </summary>
    public class ChangeTrackingTriggersOptions
    {
        /// <summary>
        /// Gets or sets a function that overrides the trigger name with the given format.
        /// </summary>
        public Func<string, string>? TriggerNameFactory { get; set; }

        internal static ChangeTrackingTriggersOptions Create(Action<ChangeTrackingTriggersOptions>? builder = null)
        {
            var options = new ChangeTrackingTriggersOptions();
            builder?.Invoke(options);
            return options;
        }
    }
}
