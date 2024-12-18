using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EFCore.ChangeTriggers.Exceptions
{
    internal class ExceptionStrings
    {
        public static string PropertyTypeDoesNotMatch(string entityName, string propertyName, string entityClrTypeName, string configuredClrTypeName)
            => $"The type of property {entityName}.{propertyName} ({entityClrTypeName}) does not match " +
                $"the type configured on the DbContext ({configuredClrTypeName}).";
        public static string NoPrimaryKeyConfigured(string entityName)
            => $"{entityName} must have a primary key configured to use change triggers.";

        public static string MoreThanOnePrimaryKeyConfigured(string entityName)
            => $"{entityName} must only have a single primary key property to use change triggers.";

        public static string NoTrackedEntityForeignKeyConfigured(string entityName)
            => $"{entityName} has no tracked entity foreign key configured.";

        public static string TriggerNameContainsFormatString()
            => "Change trigger name cannot contain the value '{{0}}'.";

        public static string TriggerNameForEntityContainsFormatString(string entityName)
            => $"Change trigger name for entity type '{entityName}' cannot contain the value '{{0}}'.";

        public static string InterceptorDbContextNotSet
            => "No DbContext available to use in the DbConnectionInterceptor.";
    }
}