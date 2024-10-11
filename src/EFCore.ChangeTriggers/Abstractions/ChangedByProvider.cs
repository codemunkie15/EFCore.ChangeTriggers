namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// Provides the change context for who changes are made by.
    /// </summary>
    /// <typeparam name="TChangedBy">The changed by type (either a static value e.g. a username string, or an EF Core entity).</typeparam>
    public abstract class ChangedByProvider<TChangedBy> : IChangedByProvider<TChangedBy>
    {
        public virtual TChangedBy GetChangedBy()
        {
            throw new NotImplementedException(
                $"Your ChangedByProvider does not implement the {nameof(IChangedByProvider<TChangedBy>.GetChangedBy)}() method.");
        }

        public virtual Task<TChangedBy> GetChangedByAsync()
        {
            throw new NotImplementedException(
                $"Your ChangedByProvider does not implement the {nameof(IChangedByProvider<TChangedBy>.GetChangedByAsync)}() method.");
        }

        public virtual TChangedBy GetMigrationChangedBy()
        {
            return GetChangedBy();
        }

        public async virtual Task<TChangedBy> GetMigrationChangedByAsync()
        {
            return await GetChangedByAsync();
        }
    }
}
