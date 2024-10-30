namespace EFCore.ChangeTriggers.ChangeEventQueries.Exceptions
{
    internal static class ExceptionStrings
    {
        public static string QueryableInvalidElementType(string typeName)
            => $"The provided IQueryable must be an IQueryable<{typeName}>.";
    }
}