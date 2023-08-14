using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class ModelExtensions
    {
        public static object? GetRawValue(this IModel model, object? value, Type valueType)
        {
            if (value is null)
            {
                return null;
            }

            var entityType = model.FindEntityType(valueType);

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

        public static object? GetRawValue<TValue>(this IModel model, object? value)
        {
            return model.GetRawValue(value, typeof(TValue));
        }

        public static Type GetRawValueType(this IModel model, Type valueType)
        {
            var entityType = model.FindEntityType(valueType);

            if (entityType is not null)
            {
                // Use the entity primary key
                var primaryKeyProperty = entityType.GetSinglePrimaryKeyProperty();
                return primaryKeyProperty.ClrType;
            }
            else
            {
                // Use the literal value
                return valueType;
            }
        }
    }
}
