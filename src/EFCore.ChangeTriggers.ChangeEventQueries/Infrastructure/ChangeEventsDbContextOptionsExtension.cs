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
                var configuration = BuildConfiguration(ConfigurationsAssembly);
                services.AddScoped(_ => configuration);
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

        private static ChangeEventConfiguration BuildConfiguration(Assembly configurationsAssembly)
        {
            var builder = new ChangeEventConfigurationBuilder();

            var test = builder.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(m => m.Name == nameof(ChangeEventConfigurationBuilder.Configure)).ToList();

            var builderConfigureMethod = builder.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Single(m => m.Name == nameof(ChangeEventConfigurationBuilder.Configure) &&
                             m.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IChangeEventEntityConfiguration<>));
            
            var configurationTypes = configurationsAssembly
                .GetTypes()
                .Where(type =>!type.IsAbstract &&
                       type.GetConstructor(Type.EmptyTypes) != null &&
                       type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IChangeEventEntityConfiguration<>)));

            foreach (var configurationType in configurationTypes)
            {
                var @interface = configurationType.GetInterfaces()
                    .First(i => i.GetGenericTypeDefinition() == typeof(IChangeEventEntityConfiguration<>));

                // Call configure on the instance, passing the created builder
                var target = builderConfigureMethod.MakeGenericMethod(@interface.GenericTypeArguments[0]);
                target.Invoke(builder, [Activator.CreateInstance(configurationType)]);
            }

            return builder.Build();
        }
    }
}
