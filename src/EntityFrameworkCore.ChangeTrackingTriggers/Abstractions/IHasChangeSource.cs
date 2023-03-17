namespace EntityFrameworkCore.ChangeTrackingTriggers.Abstractions
{
    public interface IHasChangeSource<TChangeSource>
    {
        TChangeSource ChangeSource { get; set; }
    }
}
