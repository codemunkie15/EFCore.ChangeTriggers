using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Migrations.Migrators;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EFCore.ChangeTriggers.Infrastructure
{
    public abstract class ChangeTriggersDbContextOptionsExtension : IDbContextOptionsExtension
    {
        public abstract DbContextOptionsExtensionInfo Info { get; }

        private readonly List<ServiceDescriptor> serviceDescriptors = [];

        protected ChangeTriggersDbContextOptionsExtension()
        {

        }

        public ChangeTriggersDbContextOptionsExtension(ChangeTriggersDbContextOptionsExtension copyFrom)
        {
            TriggerNameFactory = copyFrom.TriggerNameFactory;
            serviceDescriptors = copyFrom.serviceDescriptors;
        }

        public Func<string, string>? TriggerNameFactory { get; private set; }

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

        public ChangeTriggersDbContextOptionsExtension WithChangedBy<TChangedByProvider, TChangedBy>()
        {
            var clone = Clone();
            clone.AddChangedByServices<TChangedByProvider, TChangedBy>();
            return clone;
        }

        public ChangeTriggersDbContextOptionsExtension WithChangeSource<TChangeSourceProvider, TChangeSource>()
        {
            var clone = Clone();
            clone.AddChangeSourceServices<TChangeSourceProvider, TChangeSource>();
            return clone;
        }

        protected virtual void AddChangedByServices<TChangedByProvider, TChangedBy>()
        {
            AddService<ISetChangeContextOperationGenerator, ChangedBySetChangeContextOperationGenerator<TChangedBy>>();
            AddService<IChangedByProvider<TChangedBy>, TChangedByProvider>();
        }

        protected virtual void AddChangeSourceServices<TChangeSourceProvider, TChangeSource>()
        {
            AddService<ISetChangeContextOperationGenerator, ChangeSourceSetChangeContextOperationGenerator<TChangeSource>>();
            AddService<IChangeSourceProvider<TChangeSource>, TChangeSourceProvider>();
        }

        protected void AddService<TService, TImplementation>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            serviceDescriptors.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));
        }

        protected abstract ChangeTriggersDbContextOptionsExtension Clone();
    }
}
