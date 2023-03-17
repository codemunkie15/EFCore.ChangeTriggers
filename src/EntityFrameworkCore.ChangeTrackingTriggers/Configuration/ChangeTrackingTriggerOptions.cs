namespace EntityFrameworkCore.ChangeTrackingTriggers.Configuration
{
    public class ChangeTrackingTriggerOptions
    {
        public Func<string, string>? TriggerNameFactory { get; set; }

        internal static ChangeTrackingTriggerOptions Create(Action<ChangeTrackingTriggerOptions>? optionsBuilder = null)
        {
            var options = new ChangeTrackingTriggerOptions();
            optionsBuilder?.Invoke(options);
            return options;
        }
    }
}
