namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal class PropertyQueryBuilder
        : BasePropertyQueryBuilder<ChangeEvent>
    {
        public PropertyQueryBuilder(IQueryable query) : base(query)
        {
        }
    }
}