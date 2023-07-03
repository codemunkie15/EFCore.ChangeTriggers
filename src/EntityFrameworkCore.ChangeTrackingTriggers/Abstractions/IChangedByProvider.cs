namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface IChangedByProvider<TChangedBy>
    {
        Task<TChangedBy> GetChangedByAsync();
    }
}
