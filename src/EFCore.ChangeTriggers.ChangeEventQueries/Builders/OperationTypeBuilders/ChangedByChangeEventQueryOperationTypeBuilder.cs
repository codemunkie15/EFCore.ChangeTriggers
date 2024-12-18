using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal class ChangedByChangeEventQueryOperationTypeBuilder<TChangedBy>
        : BaseChangeEventQueryOperationTypeBuilder<ChangeEvent<TChangedBy>>
    {
        public ChangedByChangeEventQueryOperationTypeBuilder(IQueryable query) : base(query)
        {
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(Expression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangedBy, Expression.Property(changeEntity, nameof(IHasChangedBy<_>.ChangedBy)));
        }
    }
}