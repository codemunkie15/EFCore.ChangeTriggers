namespace EFCore.ChangeTriggers.Configuration.ChangeTriggers
{
    /// <inheritdoc/>
    public class ChangedByChangeTriggersOptions<TChangedBy> : ChangeTriggersOptions, IChangedByChangeTriggersOptions<TChangedBy>
    {
        internal static ChangedByChangeTriggersOptions<TChangedBy> Create(Action<ChangedByChangeTriggersOptions<TChangedBy>>? builder = null)
        {
            var options = new ChangedByChangeTriggersOptions<TChangedBy>();
            builder?.Invoke(options);
            return options;
        }
    }
}
