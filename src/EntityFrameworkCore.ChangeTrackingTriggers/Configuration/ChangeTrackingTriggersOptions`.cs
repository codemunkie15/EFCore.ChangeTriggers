namespace EntityFrameworkCore.ChangeTrackingTriggers.Configuration
{
    public class ChangeTrackingTriggersOptions<TChangeSource> : ChangeTrackingTriggersOptions
    {
        public TChangeSource? MigrationSourceType { get; set; }

        internal static ChangeTrackingTriggersOptions<TChangeSource> Create(Action<ChangeTrackingTriggersOptions<TChangeSource>>? builder = null)
        {
            var options = new ChangeTrackingTriggersOptions<TChangeSource>();
            builder?.Invoke(options);
            return options;
        }
    }
}
