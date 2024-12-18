using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Extensions
{
    internal static class ExpressionExtensions
    {
        public static MethodCallExpression ApplySelect(this Expression source, Expression selector, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Select),
                [parameter.Type, selector.Type],
                source,
                Expression.Lambda(selector, parameter));
        }

        public static MethodCallExpression ApplyWhere(this Expression source, Expression predicate, ParameterExpression parameter)
        {
            return Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Where),
                [parameter.Type],
                source,
                Expression.Lambda(predicate, parameter));
        }
    }
}
