using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries
{
    internal class ParameterReplaceVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression oldParam;
        private readonly Expression newExpression;

        public ParameterReplaceVisitor(ParameterExpression oldParam, Expression newExpression)
        {
            this.oldParam = oldParam;
            this.newExpression = newExpression;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == oldParam
                ? newExpression
                : base.VisitParameter(node);
        }
    }
}