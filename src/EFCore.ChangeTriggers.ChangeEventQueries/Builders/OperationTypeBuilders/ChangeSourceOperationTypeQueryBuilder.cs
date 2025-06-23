using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangeSourceEvents;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.OperationTypeBuilders
{
    internal class ChangeSourceOperationTypeQueryBuilder<TChangeSource>
        : BaseOperationTypeQueryBuilder<ChangeEvent<TChangeSource>>
    {
        public ChangeSourceOperationTypeQueryBuilder(IQueryable query) : base(query)
        {
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(Expression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangeSource, Expression.Property(changeEntity, nameof(IHasChangeSource<_>.ChangeSource)));
        }
    }
}
