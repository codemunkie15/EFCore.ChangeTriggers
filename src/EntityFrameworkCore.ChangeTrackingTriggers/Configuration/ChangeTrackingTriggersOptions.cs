namespace EntityFrameworkCore.ChangeTrackingTriggers.Configuration
{
    public class ChangeTrackingTriggersOptions
    {
        public Func<string, string>? TriggerNameFactory { get; set; }

        internal static ChangeTrackingTriggersOptions Create(Action<ChangeTrackingTriggersOptions>? builder = null)
        {
            var options = new ChangeTrackingTriggersOptions();
            builder?.Invoke(options);
            return options;
        }
    }
}
