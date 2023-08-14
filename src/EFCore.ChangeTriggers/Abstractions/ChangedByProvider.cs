namespace EFCore.ChangeTriggers.Abstractions
{
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
