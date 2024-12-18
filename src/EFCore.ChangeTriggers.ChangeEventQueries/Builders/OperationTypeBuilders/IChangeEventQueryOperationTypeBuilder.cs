namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal interface IChangeEventQueryOperationTypeBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        IQueryable<TChangeEvent> Build(OperationType operationType);
    }
}