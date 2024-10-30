namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal class ChangeEventQueryPropertyBuilder
        : BaseChangeEventQueryPropertyBuilder<ChangeEvent>
    {
        public ChangeEventQueryPropertyBuilder(IQueryable query) : base(query)
        {
        }
    }
}