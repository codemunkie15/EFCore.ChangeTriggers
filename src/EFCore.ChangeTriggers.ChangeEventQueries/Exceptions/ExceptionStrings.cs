namespace EFCore.ChangeTriggers.ChangeEventQueries.Exceptions
{
    internal static class ExceptionStrings
    {
        public static string IQueryableInvalidElementType(string typeName)
            => $"The provided IQueryable must be an IQueryable<{typeName}>.";

        public static string IQueryableDoesNotImplementCorrectProvider()
            => "IQueryable does not implement the correct provider. Make sure ChangeEventQueries is configured on your DbContext.";

        public static string ChangeEventsDbContextOptionsExtensionNotFound()
            => "ChangeEventsDbContextOptionsExtension could not be found. Make sure ChangeEventQueries is configured on your DbContext.";

        public static string LambdaExpressionInvalidMember(string? typeName = null)
        {
            var entityText = !string.IsNullOrEmpty(typeName) ? $" on the type {typeName}" : "";
            return $"The lambda expression does not refer to a valid member{entityText}.";
        }

        public static string EntityConfigurationNotFound(string typeName)
            => $"No configuration found for entity type {typeName}.";

        public static string ConfigurationNotFoundFromDbContext()
            => "No configuration found. Either pass a configuration in ToChangeEvents() or auto-register configurations via your DbContext.";

        public static string ConfigurationAlreadyAdded(string typeName)
            => $"A configuration for entity type {typeName} has already been added.";
    }
}