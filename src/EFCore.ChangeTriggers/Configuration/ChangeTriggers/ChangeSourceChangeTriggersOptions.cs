namespace EFCore.ChangeTriggers.Configuration.ChangeTriggers
{
    /// <inheritdoc/>
    public class ChangeSourceChangeTriggersOptions<TChangeSource> : ChangeTriggersOptions, IChangeSourceChangeTriggersOptions<TChangeSource>
    {
        internal static ChangeSourceChangeTriggersOptions<TChangeSource> Create(Action<ChangeSourceChangeTriggersOptions<TChangeSource>>? builder = null)
        {
            var options = new ChangeSourceChangeTriggersOptions<TChangeSource>();
            builder?.Invoke(options);
            return options;
        }
    }
}
