using EFCore.ChangeTriggers.ChangeEventQueries.Configuration;
using EFCore.ChangeTriggers.ChangeEventQueries.Configuration.Builders;
using System.Reflection;

namespace EFCore.ChangeTriggers.ChangeEventQueries.Builders
{
    internal class AssemblyConfigurationBuilder : IAssemblyConfigurationBuilder
    {
        public ChangeEventConfiguration Build(Assembly configurationsAssembly)
        {
            var builder = new ChangeEventConfigurationBuilder();

            var builderConfigureMethod = builder.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                .Single(m => m.Name == nameof(ChangeEventConfigurationBuilder.Configure) &&
                             m.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() == typeof(IChangeEventEntityConfiguration<>));

            var configurationTypes = configurationsAssembly
                .GetTypes()
                .Where(type => !type.IsAbstract &&
                       !type.ContainsGenericParameters &&
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
