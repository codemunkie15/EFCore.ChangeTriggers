namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface IHasChangedBy<TChangedByType>
    {
        TChangedByType ChangedBy { get; set; }
    }
}
