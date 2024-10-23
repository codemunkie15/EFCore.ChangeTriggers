namespace EFCore.ChangeTriggers.Abstractions
{
    /// <summary>
    /// Represents a change entity that tracks the changes of a <see cref="TTracked"/> entity.
    /// </summary>
    /// <typeparam name="TTracked">The entity type that this change entity tracks.</typeparam>
    public interface IChange<TTracked> : IChange, IHasTrackedEntity<TTracked>
    {
    }
}
