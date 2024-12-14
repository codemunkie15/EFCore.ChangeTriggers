using EFCore.ChangeTriggers.ChangeEventQueries.Exceptions;
using System.Linq.Expressions;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Extensions
{
    internal static class LambdaExpressionExtensions
    {
        public static string GetRootMemberName(this LambdaExpression expression)
        {
            var body = expression.Body;

            while (body is MethodCallExpression methodCall)
            {
                body = methodCall.Object;
            }

            if (body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            var typeName = expression.Parameters.FirstOrDefault()?.Type.Name;
            throw new ChangeEventConfigurationException(ExceptionStrings.LambdaExpressionInvalidMember(typeName));
        }
    }
}
