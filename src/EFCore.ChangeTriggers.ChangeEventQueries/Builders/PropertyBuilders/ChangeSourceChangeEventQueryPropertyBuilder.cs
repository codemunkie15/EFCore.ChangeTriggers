using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal class ChangeSourceChangeEventQueryPropertyBuilder<TChangeSource>
        : BaseChangeEventQueryPropertyBuilder<ChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryPropertyBuilder(IQueryable query) : base(query)
        {
            query.EnsureElementType<IHasChangeSource<TChangeSource>>();
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(MemberExpression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangeSource, Expression.Property(changeEntity, nameof(IHasChangeSource<_>.ChangeSource)));
        }
    }
}