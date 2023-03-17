using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.ChangeTrackingTriggers.EfCoreExtension
{
    internal class ChangeTrackingExtension : IDbContextOptionsExtension
    {
        public DbContextOptionsExtensionInfo Info { get; }

        private readonly Action<IServiceCollection> servicesBuilder;

        public ChangeTrackingExtension(Action<IServiceCollection> servicesBuilder)
        {
            this.servicesBuilder = servicesBuilder;
            Info = new ChangeTrackingExtensionInfo(this);
        }

        public void ApplyServices(IServiceCollection services)
        {
            servicesBuilder(services);
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}
