namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface IChangedByProvider<TChangedById>
    {
        Task<TChangedById> GetChangedById();
    }
}
