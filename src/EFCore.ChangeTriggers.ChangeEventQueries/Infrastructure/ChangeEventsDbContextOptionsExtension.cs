using EFCore.ChangeTriggers.ChangeEventQueries.Builders;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure
{
    public class ChangeEventsDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public Assembly? ConfigurationsAssembly { get; private set; }

        private ChangeEventsDbContextOptionsExtensionInfo? info;

        public DbContextOptionsExtensionInfo Info =>
            info ??= new ChangeEventsDbContextOptionsExtensionInfo(this);

        public ChangeEventsDbContextOptionsExtension()
        {

        }

        public ChangeEventsDbContextOptionsExtension(ChangeEventsDbContextOptionsExtension copyFrom)
        {
            ConfigurationsAssembly = copyFrom.ConfigurationsAssembly;
        }

        public virtual void ApplyServices(IServiceCollection services)
        {
            if (ConfigurationsAssembly != null)
            {
                services.AddScoped<IAssemblyConfigurationBuilder, AssemblyConfigurationBuilder>();
                services.AddScoped(services => services.GetRequiredService<IAssemblyConfigurationBuilder>().Build(ConfigurationsAssembly));
            }
        }

        public void Validate(IDbContextOptions options)
        {
        }

        public ChangeEventsDbContextOptionsExtension WithConfigurationsAssembly(Assembly configurationsAssembly)
        {
            var clone = Clone();
            clone.ConfigurationsAssembly = configurationsAssembly;
            return clone;
        }

        private ChangeEventsDbContextOptionsExtension Clone()
        {
            return new ChangeEventsDbContextOptionsExtension(this);
        }
    }
}
