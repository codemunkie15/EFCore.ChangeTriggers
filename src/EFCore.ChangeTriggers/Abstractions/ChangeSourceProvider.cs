namespace EFCore.ChangeTriggers.Abstractions
{
    public abstract class ChangeSourceProvider<TChangeSource> : IChangeSourceProvider<TChangeSource>
    {
        public virtual TChangeSource GetChangeSource()
        {
            throw new NotImplementedException(
                $"Your ChangeSourceProvider does not implement the {nameof(IChangeSourceProvider<TChangeSource>.GetChangeSource)}() method.");
        }

        public virtual Task<TChangeSource> GetChangeSourceAsync()
        {
            throw new NotImplementedException(
                $"Your ChangeSourceProvider does not implement the {nameof(IChangeSourceProvider<TChangeSource>.GetChangeSourceAsync)}() method.");
        }

        public virtual TChangeSource GetMigrationChangeSource()
        {
            return GetChangeSource();
        }

        public async virtual Task<TChangeSource> GetMigrationChangeSourceAsync()
        {
            return await GetChangeSourceAsync();
        }
    }
}
