using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal class ChangeEventQueryPropertyBuilder<TChangedBy, TChangeSource>
        : BaseChangeEventQueryPropertyBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public ChangeEventQueryPropertyBuilder(IQueryable query) : base(query)
        {
            query.EnsureElementType<IHasChangedBy<TChangedBy>>();
            query.EnsureElementType<IHasChangeSource<TChangeSource>>();
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(Expression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangedBy, Expression.Property(changeEntity, nameof(IHasChangedBy<_>.ChangedBy)));
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangeSource, Expression.Property(changeEntity, nameof(IHasChangeSource<_>.ChangeSource)));
        }
    }
}