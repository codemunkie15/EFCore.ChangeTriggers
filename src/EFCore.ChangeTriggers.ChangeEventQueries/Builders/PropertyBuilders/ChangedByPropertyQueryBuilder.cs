using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.ChangeEventQueries.ChangedBy;
using EFCore.ChangeTriggers.ChangeEventQueries.Extensions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders.PropertyBuilders
{
    internal class ChangedByPropertyQueryBuilder<TChangedBy>
        : BasePropertyQueryBuilder<ChangeEvent<TChangedBy>>
    {
        public ChangedByPropertyQueryBuilder(IQueryable query) : base(query)
        {
            query.EnsureElementType<IHasChangedBy<TChangedBy>>();
        }

        protected override IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(Expression changeEntity)
        {
            yield return BuildChangeEventPropertyBinding(ce => ce.ChangedBy, Expression.Property(changeEntity, nameof(IHasChangedBy<_>.ChangedBy)));
        }
    }
}