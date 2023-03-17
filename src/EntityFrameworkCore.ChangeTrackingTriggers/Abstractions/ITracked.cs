namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface ITracked<TChangeType>
    {
        ICollection<TChangeType> Changes { get; set; }
    }
}
