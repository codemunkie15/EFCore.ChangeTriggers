namespace EFCore.ChangeTriggers.Configuration
{
    /// <inheritdoc/>
    public class ChangeTriggersOptions<TChangeSource> : ChangeTriggersOptions
    {
        /// <summary>
        /// Gets or sets the source type that indicates an EF Core migration.
        /// </summary>
        public TChangeSource? MigrationSourceType { get; set; }

        internal static ChangeTriggersOptions<TChangeSource> Create(Action<ChangeTriggersOptions<TChangeSource>>? builder = null)
        {
            var options = new ChangeTriggersOptions<TChangeSource>();
            builder?.Invoke(options);
            return options;
        }
    }
}
