using EFCore.ChangeTriggers.ChangeEventQueries.Builders;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Infrastructure
{
    public class ChangeEventsDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public Assembly? ConfigurationsAssembly { get; private set; }

        public bool IncludeInserts { get; private set; }

        public bool IncludeDeletes { get; private set; }

        private ChangeEventsDbContextOptionsExtensionInfo? info;

        public DbContextOptionsExtensionInfo Info =>
            info ??= new ChangeEventsDbContextOptionsExtensionInfo(this);

        public ChangeEventsDbContextOptionsExtension()
        {

        }

        public ChangeEventsDbContextOptionsExtension(ChangeEventsDbContextOptionsExtension copyFrom)
        {
            ConfigurationsAssembly = copyFrom.ConfigurationsAssembly;
            IncludeInserts = copyFrom.IncludeInserts;
            IncludeDeletes = copyFrom.IncludeDeletes;
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

        public ChangeEventsDbContextOptionsExtension WithIncludeInserts(bool include)
        {
            var clone = Clone();
            clone.IncludeInserts = include;
            return clone;
        }

        public ChangeEventsDbContextOptionsExtension WithIncludeDeletes(bool include)
        {
            var clone = Clone();
            clone.IncludeDeletes = include;
            return clone;
        }

        private ChangeEventsDbContextOptionsExtension Clone()
        {
            return new ChangeEventsDbContextOptionsExtension(this);
        }
    }
}
