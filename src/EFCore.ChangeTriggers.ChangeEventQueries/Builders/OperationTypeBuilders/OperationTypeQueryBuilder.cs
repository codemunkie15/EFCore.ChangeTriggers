namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal class OperationTypeQueryBuilder : BaseOperationTypeQueryBuilder<ChangeEvent>
    {
        public OperationTypeQueryBuilder(IQueryable query) : base(query)
        {
        }
    }
}
