namespace EntityFrameworkCore.ChangeTrackingTriggers.Configuration
{
    /// <inheritdoc/>
    public class ChangeTrackingTriggersOptions<TChangeSource> : ChangeTrackingTriggersOptions
    {
        /// <summary>
        /// Gets or sets the source type that indicates an EF Core migration.
        /// </summary>
        public TChangeSource? MigrationSourceType { get; set; }

        internal static ChangeTrackingTriggersOptions<TChangeSource> Create(Action<ChangeTrackingTriggersOptions<TChangeSource>>? builder = null)
        {
            var options = new ChangeTrackingTriggersOptions<TChangeSource>();
            builder?.Invoke(options);
            return options;
        }
    }
}
