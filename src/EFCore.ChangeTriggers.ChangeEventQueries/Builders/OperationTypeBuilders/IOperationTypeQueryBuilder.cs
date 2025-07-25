﻿namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal interface IOperationTypeQueryBuilder<TChangeEvent>
        where TChangeEvent : ChangeEvent
    {
        IQueryable<TChangeEvent> Build(OperationType operationType);
    }
}