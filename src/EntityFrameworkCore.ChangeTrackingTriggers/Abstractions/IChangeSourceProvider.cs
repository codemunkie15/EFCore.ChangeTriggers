namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface IChangeSourceProvider<TChangeSource>
    {
        Task<TChangeSource> GetChangeSource();
    }
}
