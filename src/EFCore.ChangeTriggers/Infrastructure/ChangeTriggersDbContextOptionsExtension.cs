using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Migrations.Migrators;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Infrastructure
{
    public abstract class ChangeTriggersDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public abstract DbContextOptionsExtensionInfo Info { get; }

        public Func<string, string>? TriggerNameFactory { get; private set; }

        public ChangeContextTypes ChangedByTypes { get; private set; }

        public ChangeContextTypes ChangeSourceTypes { get; private set; }

        private readonly List<ServiceDescriptor> serviceDescriptors = [];

        protected ChangeTriggersDbContextOptionsExtension()
        {

        }

        public ChangeTriggersDbContextOptionsExtension(ChangeTriggersDbContextOptionsExtension copyFrom)
        {
            TriggerNameFactory = copyFrom.TriggerNameFactory;
            serviceDescriptors = copyFrom.serviceDescriptors;
        }

        public virtual void ApplyServices(IServiceCollection services)
        {
            services.AddSingleton(new ChangeTriggersExtensionContext());

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

        public ChangeTriggersDbContextOptionsExtension WithChangedBy<TChangedByProvider, TChangedBy>(IServiceProvider? applicationServiceProvider)
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            var clone = Clone();
            clone.AddChangedByServices<TChangedByProvider, TChangedBy>(applicationServiceProvider);
            clone.ChangedByTypes = new ChangeContextTypes(typeof(TChangedByProvider), typeof(TChangedBy));
            return clone;
        }

        public ChangeTriggersDbContextOptionsExtension WithChangeSource<TChangeSourceProvider, TChangeSource>(IServiceProvider? applicationServiceProvider)
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            var clone = Clone();
            clone.AddChangeSourceServices<TChangeSourceProvider, TChangeSource>(applicationServiceProvider);
            clone.ChangeSourceTypes = new ChangeContextTypes(typeof(TChangeSourceProvider), typeof(TChangeSource));
            return clone;
        }

        protected virtual void AddChangedByServices<TChangedByProvider, TChangedBy>(IServiceProvider? applicationServiceProvider)
            where TChangedByProvider : class, IChangedByProvider<TChangedBy>
        {
            AddService<ISetChangeContextOperationGenerator, ChangedBySetChangeContextOperationGenerator<TChangedBy>>();

            // Services that may depend on application services need to be registered differently
            AddService<IChangedByProvider<TChangedBy>, TChangedByProvider>(applicationServiceProvider);
        }

        protected virtual void AddChangeSourceServices<TChangeSourceProvider, TChangeSource>(IServiceProvider? applicationServiceProvider)
            where TChangeSourceProvider : class, IChangeSourceProvider<TChangeSource>
        {
            AddService<ISetChangeContextOperationGenerator, ChangeSourceSetChangeContextOperationGenerator<TChangeSource>>();

            // Services that may depend on application services need to be registered differently
            AddService<IChangeSourceProvider<TChangeSource>, TChangeSourceProvider>(applicationServiceProvider);
        }

        protected void AddService<TService, TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
        }

        protected void AddService<TService, TImplementation>(IServiceProvider? applicationServiceProvider, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TImplementation : class, TService
        {
            if (applicationServiceProvider != null)
            {
                // Attempt to resolve/create the service from the application service provider, incase it has application specific dependencies.
                // Hopefully soon EF Core will have a better way to deal with this scenario that doesn't require reflection.
                serviceDescriptors.Add(new ServiceDescriptor(
                    typeof(TService),
                    services =>
                    {
                        return ActivatorUtilities.GetServiceOrCreateInstance<TImplementation>(applicationServiceProvider);
                    },
                    serviceLifetime));
            }
            else
            {
                // If no application service provider is available, just register the service in the EF Core provider.
                AddService<TService, TImplementation>(serviceLifetime);
            }
        }

        protected abstract ChangeTriggersDbContextOptionsExtension Clone();
    }

    public record ChangeContextTypes(Type ProviderType, Type valueType);
}
