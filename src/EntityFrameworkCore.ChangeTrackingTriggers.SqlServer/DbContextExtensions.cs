using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer
{
    internal static class DbContextExtensions
    {
        public static object? ConvertToSqlValue<TClrType>(this DbContext context, object? value)
        {
            var typeMapping = (RelationalTypeMapping?)context?.Model.GetModelDependencies().TypeMappingSource.FindMapping(typeof(TClrType));

            return typeMapping?.Converter is not null
                ? typeMapping.Converter.ConvertToProvider(value)
                : value;
        }
    }
}
