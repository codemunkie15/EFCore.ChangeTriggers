using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.EfCoreExtension
{
    internal class ChangeTriggersExtension : IDbContextOptionsExtension
    {
        public DbContextOptionsExtensionInfo Info { get; }

        private readonly Action<IServiceCollection> servicesBuilder;

        public ChangeTriggersExtension(Action<IServiceCollection> servicesBuilder)
        {
            this.servicesBuilder = servicesBuilder;
            Info = new ChangeTriggersExtensionInfo(this);
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
