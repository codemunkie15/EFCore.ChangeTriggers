namespace EFCore.ChangeTriggers.ChangeEventQueries.Exceptions
{
    internal static class ExceptionStrings
    {
        public static string QueryableInvalidElementType(string typeName)
            => $"The provided IQueryable must be an IQueryable<{typeName}>.";

        public static string LambdaExpressionInvalidMember(string? typeName = null)
        {
            var entityText = !string.IsNullOrEmpty(typeName) ? $" on the type {typeName}" : "";
            return $"The lambda expression does not refer to a valid member{entityText}.";
        }
    }
}