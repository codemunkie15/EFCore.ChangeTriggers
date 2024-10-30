using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Configuration
{
    public class ChangeEventQueryConfiguration
    {
        public IDictionary<Type, IEnumerable<LambdaExpression>> Configurations => configurations.AsReadOnly();

        private readonly Dictionary<Type, IEnumerable<LambdaExpression>> configurations = [];

        public ChangeEventQueryConfiguration ForEntity<TChange>(Action<ChangeEventQueryEntityConfiguration<TChange>> configure)
        {
            var builder = new ChangeEventQueryEntityConfiguration<TChange>();
            configure(builder);

            configurations.Add(typeof(TChange), builder.ValueSelectors);

            return this;
        }
    }

    public class ChangeEventQueryEntityConfiguration<TChange>
    {
        public IEnumerable<Expression<Func<TChange, string>>> ValueSelectors => valueSelectors.AsReadOnly();

        private readonly List<Expression<Func<TChange, string>>> valueSelectors = [];

        public ChangeEventQueryEntityConfiguration<TChange> AddProperty(Expression<Func<TChange, string>> valueSelector)
        {
            valueSelectors.Add(valueSelector);

            return this;
        }
    }
}
