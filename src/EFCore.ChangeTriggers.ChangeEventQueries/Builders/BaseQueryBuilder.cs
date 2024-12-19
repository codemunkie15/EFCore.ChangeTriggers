using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal abstract class BaseQueryBuilder<TChangeEvent>
    {
        protected MemberBinding BuildChangeEventPropertyBinding<TResult>(Expression<Func<TChangeEvent, TResult>> memberExpression, Expression valueExpression)
        {
            var memberInfo = ((MemberExpression)memberExpression.Body).Member;
            return Expression.Bind(memberInfo, valueExpression);
        }

        protected virtual IEnumerable<MemberBinding> GetAdditionalChangeEventPropertyBindings(Expression changeEntity) => [];
    }
}
