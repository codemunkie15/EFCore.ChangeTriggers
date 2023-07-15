namespace EntityFrameworkCore.ChangeTrackingTriggers.Configuration
{
    /// <summary>
    /// Provides an API for configuring a single change tracking trigger.
    /// </summary>
    public class ChangeTrackingTriggerOptions
    {
        /// <summary>
        /// Gets or sets a function that overrides the trigger name with the given format.
        /// </summary>
        public Func<string, string>? TriggerNameFactory { get; set; }

        internal static ChangeTrackingTriggerOptions Create(Action<ChangeTrackingTriggerOptions>? optionsBuilder = null)
        {
            var options = new ChangeTrackingTriggerOptions();
            optionsBuilder?.Invoke(options);
            return options;
        }
    }
}
