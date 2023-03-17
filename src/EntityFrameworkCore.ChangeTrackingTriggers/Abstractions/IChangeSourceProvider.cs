namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface IChangeSourceProvider<TSourceType>
    {
        Task<TSourceType> GetSourceTypeAsync();
    }
}
