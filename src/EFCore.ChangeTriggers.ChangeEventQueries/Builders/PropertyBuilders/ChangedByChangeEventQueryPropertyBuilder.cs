using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal class ChangedByChangeEventQueryPropertyBuilder<TChangedBy>
        : BaseChangeEventQueryPropertyBuilder<ChangeEvent<TChangedBy>>
    {
        public ChangedByChangeEventQueryPropertyBuilder(IQueryable query) : base(query)
        {
            query.EnsureElementType<IHasChangedBy<TChangedBy>>();
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(MemberExpression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangedBy, Expression.Property(changeEntity, nameof(IHasChangedBy<object>.ChangedBy)));
        }
    }
}