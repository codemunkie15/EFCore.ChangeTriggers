namespace EFCore.ChangeTriggers.Configuration
{
    /// <summary>
    /// Provides an API for configuring a single change trigger.
    /// </summary>
    public class ChangeTriggerOptions
    {
        /// <summary>
        /// Gets or sets a function that overrides the trigger name with the given format.
        /// </summary>
        public Func<string, string>? TriggerNameFactory { get; set; }

        internal static ChangeTriggerOptions Create(Action<ChangeTriggerOptions>? optionsBuilder = null)
        {
            var options = new ChangeTriggerOptions();
            optionsBuilder?.Invoke(options);
            return options;
        }
    }
}
