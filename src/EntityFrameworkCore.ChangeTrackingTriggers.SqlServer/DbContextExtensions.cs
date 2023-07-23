using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer
{
    internal static class DbContextExtensions
    {
        public static object? ConvertToProvider<TClrType>(this DbContext context, object? value)
        {
            var typeMapping = context?.Model.GetModelDependencies().TypeMappingSource.FindMapping(typeof(TClrType));

            return typeMapping?.Converter is not null
                ? typeMapping.Converter.ConvertToProvider(value)
                : value;
        }
    }
}
