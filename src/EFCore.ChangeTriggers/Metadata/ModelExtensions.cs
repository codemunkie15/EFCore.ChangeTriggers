using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Metadata
{
    internal static class ModelExtensions
    {
        public static object? GetRawValue(this IModel model, object? value)
        {
            if (value is null)
            {
                return null;
            }

            var entityType = model.FindEntityType(value.GetType());

            if (entityType is not null)
            {
                // Use the entity primary key
                var primaryKeyProperty = entityType.GetSinglePrimaryKeyProperty();
                return primaryKeyProperty.GetGetter().GetClrValue(value);
            }
            else
            {
                // Use the literal value
                return value;
            }
        }

        public static object? ConvertToProvider(this IModel model, object? value)
        {
            if (value == null)
            {
                return null;
            }

            var test = model.GetModelDependencies().TypeMappingSource.FindMapping(value.GetType(), model);

            return test?.Converter?.ConvertToProvider(value) ?? value;
        }
    }
}
