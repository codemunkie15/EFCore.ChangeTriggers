using EFCore.ChangeTriggers.Infrastructure;
using EFCore.ChangeTriggers.Metadata;
using EFCore.ChangeTriggers.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

            var options = context.GetService<IDbContextOptions>().Extensions.OfType<ChangeTriggersDbContextOptionsExtension>().Single()!;

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
                    builder.HasChangedByInternal(options.ChangedByConfig!.EntityClrType);
                }

                if (changeEntityType.HasChangeSource())
                {
                    builder.HasChangeSourceInternal(options.ChangeSourceConfig!.EntityClrType);
                }
            }
        }
    }
}
