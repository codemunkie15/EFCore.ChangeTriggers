using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Exceptions;
using EFCore.ChangeTriggers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCore.ChangeTriggers
{
    internal class ChangeTriggersModelCustomizer : RelationalModelCustomizer
    {
        public ChangeTriggersModelCustomizer(ModelCustomizerDependencies dependencies) : base(dependencies)
        {
        }

        public override void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            base.Customize(modelBuilder, context);

            // Delay ChangeTriggers entity config until after context.OnModelCreating() has ran

            foreach (var trackedEntityType in modelBuilder.Model.GetEntityTypes().Where(e => e.HasChangeTrigger()))
            {
                var builder = modelBuilder.Entity(trackedEntityType.ClrType);
                builder.HasChangeTriggerInternal();
            }

            foreach (var changeEntityType in modelBuilder.Model.GetEntityTypes().Where(e => e.IsChangeTable()))
            {
                var builder = modelBuilder.Entity(changeEntityType.ClrType);
                builder.IsChangeTableInternal();

                if (changeEntityType.HasChangedBy())
                {
                    builder.HasChangedByInternal();
                }

                if (changeEntityType.HasChangeSource())
                {
                    builder.HasChangeSourceInternal();
                }
            }
        }
    }
}
