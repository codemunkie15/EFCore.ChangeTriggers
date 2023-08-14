namespace EFCore.ChangeTriggers.Configuration.ChangeTriggers
{
    /// <inheritdoc/>
    public class ChangeTriggersOptions<TChangedBy, TChangeSource> :
        ChangeTriggersOptions, IChangedByChangeTriggersOptions<TChangedBy>, IChangeSourceChangeTriggersOptions<TChangeSource>
    {
        internal static ChangeTriggersOptions<TChangedBy, TChangeSource> Create(Action<ChangeTriggersOptions<TChangedBy, TChangeSource>>? builder = null)
        {
            var options = new ChangeTriggersOptions<TChangedBy, TChangeSource>();
            builder?.Invoke(options);
            return options;
        }
    }
}
