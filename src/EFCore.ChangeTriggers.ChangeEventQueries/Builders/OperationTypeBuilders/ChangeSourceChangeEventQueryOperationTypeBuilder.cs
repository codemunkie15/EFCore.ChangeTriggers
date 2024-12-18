using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSource;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal class ChangeSourceChangeEventQueryOperationTypeBuilder<TChangeSource>
        : BaseChangeEventQueryOperationTypeBuilder<ChangeEvent<TChangeSource>>
    {
        public ChangeSourceChangeEventQueryOperationTypeBuilder(IQueryable query) : base(query)
        {
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(Expression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangeSource, Expression.Property(changeEntity, nameof(IHasChangeSource<_>.ChangeSource)));
        }
    }
}
