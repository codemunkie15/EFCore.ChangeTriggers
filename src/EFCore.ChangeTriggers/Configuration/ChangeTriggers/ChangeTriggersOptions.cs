namespace EFCore.ChangeTriggers.Configuration.ChangeTriggers
{
    /// <summary>
    /// Provides an API for configuring change triggers.
    /// </summary>
    public class ChangeTriggersOptions
    {
        /// <summary>
        /// Gets or sets a function that overrides the trigger name with the given format.
        /// </summary>
        public Func<string, string>? TriggerNameFactory { get; set; }

        internal static ChangeTriggersOptions Create(Action<ChangeTriggersOptions>? builder = null)
        {
            var options = new ChangeTriggersOptions();
            builder?.Invoke(options);
            return options;
        }
    }
}
