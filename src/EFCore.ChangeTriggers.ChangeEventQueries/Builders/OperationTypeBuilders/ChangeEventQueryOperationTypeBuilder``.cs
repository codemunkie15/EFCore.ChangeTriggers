
using EFCore.ChangeTriggers.Abstractions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal class ChangeEventQueryOperationTypeBuilder<TChangedBy, TChangeSource>
        : BaseChangeEventQueryOperationTypeBuilder<ChangeEvent<TChangedBy, TChangeSource>>
    {
        public ChangeEventQueryOperationTypeBuilder(IQueryable query) : base(query)
        {
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(Expression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangedBy, Expression.Property(changeEntity, nameof(IHasChangedBy<_>.ChangedBy)));
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangeSource, Expression.Property(changeEntity, nameof(IHasChangeSource<_>.ChangeSource)));
        }
    }
}
