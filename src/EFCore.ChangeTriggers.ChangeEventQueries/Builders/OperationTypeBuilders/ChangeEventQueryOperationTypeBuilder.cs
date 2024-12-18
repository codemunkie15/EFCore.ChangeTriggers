namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal class ChangeEventQueryOperationTypeBuilder : BaseChangeEventQueryOperationTypeBuilder<ChangeEvent>
    {
        public ChangeEventQueryOperationTypeBuilder(IQueryable query) : base(query)
        {
        }
    }
}
