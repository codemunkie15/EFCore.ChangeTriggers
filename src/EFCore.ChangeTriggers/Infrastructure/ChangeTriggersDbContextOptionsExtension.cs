using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Migrations.Operations.Generators;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Infrastructure
{
    public abstract class ChangeTriggersDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public abstract DbContextOptionsExtensionInfo Info { get; }

        public Func<string, string>? TriggerNameFactory { get; private set; }

        public ChangeContextConfig? ChangedByConfig { get; private set; }

        public ChangeContextConfig? ChangeSourceConfig { get; private set; }

        private readonly List<ServiceDescriptor> serviceDescriptors = [];

        protected ChangeTriggersDbContextOptionsExtension()
        {

        }

        public ChangeTriggersDbContextOptionsExtension(ChangeTriggersDbContextOptionsExtension copyFrom)
        {
            TriggerNameFactory = copyFrom.TriggerNameFactory;
            serviceDescriptors = copyFrom.serviceDescriptors;
            ChangedByConfig = copyFrom.ChangedByConfig;
            ChangeSourceConfig = copyFrom.ChangeSourceConfig;
        }

        public virtual void ApplyServices(IServiceCollection services)
        {
            services.AddScoped<ChangeTriggersExtensionContext>();

            foreach (var serviceDescriptor in serviceDescriptors)
            {
                services.Add(serviceDescriptor);
            }
        }

        public void Validate(IDbContextOptions options)
        {
        }

        public ChangeTriggersDbContextOptionsExtension WithTriggerNameFactory(Func<string, string> triggerNameFactory)
        {
            var clone = Clone();
            clone.TriggerNameFactory = triggerNameFactory;
            return clone;
        }

        public ChangeTriggersDbContextOptionsExtension WithChangedBy<TChangedByProvider, TChangedBy>()
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            var clone = Clone();
            clone.AddChangedByServices<TChangedByProvider, TChangedBy>();
            clone.ChangedByConfig = new ChangeContextConfig(typeof(TChangedByProvider), typeof(TChangedBy));
            return clone;
        }

        public ChangeTriggersDbContextOptionsExtension WithChangeSource<TChangeSourceProvider, TChangeSource>()
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            var clone = Clone();
            clone.AddChangeSourceServices<TChangeSourceProvider, TChangeSource>();
            clone.ChangeSourceConfig = new ChangeContextConfig(typeof(TChangeSourceProvider), typeof(TChangeSource));
            return clone;
        }

        protected virtual void AddChangedByServices<TChangedByProvider, TChangedBy>()
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            AddService<ISetChangeContextOperationGenerator, ChangedBySetChangeContextOperationGenerator<TChangedBy>>();

            // Services that may depend on application services need to be registered differently
            AddApplicationService<IChangedByProvider<TChangedBy>, TChangedByProvider>();
        }

        protected virtual void AddChangeSourceServices<TChangeSourceProvider, TChangeSource>()
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            AddService<ISetChangeContextOperationGenerator, ChangeSourceSetChangeContextOperationGenerator<TChangeSource>>();

            // Services that may depend on application services need to be registered differently
            AddApplicationService<IChangeSourceProvider<TChangeSource>, TChangeSourceProvider>();
        }

        protected void AddService<TService, TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
        }

        protected void AddApplicationService<TService, TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplementation : class, TService
        {
            // Attempt to resolve/create the service from the application service provider, incase it has application specific dependencies
            serviceDescriptors.Add(new ServiceDescriptor(
                typeof(TService),
                services =>
                {
                    var applicationServiceProvider = services
                        .GetRequiredService<IDbContextOptions>()
                        .FindExtension<CoreOptionsExtension>()?
                        .ApplicationServiceProvider;

                    return ActivatorUtilities.GetServiceOrCreateInstance<TImplementation>(applicationServiceProvider ?? services);
                },
                serviceLifetime));
        }

        protected abstract ChangeTriggersDbContextOptionsExtension Clone();
    }

    public record ChangeContextConfig(Type ProviderType, Type EntityClrType);
}
