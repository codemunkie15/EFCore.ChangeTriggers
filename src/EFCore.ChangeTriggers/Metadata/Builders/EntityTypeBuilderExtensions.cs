using EFCore.ChangeTriggers.Configuration;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Metadata.Builders
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configures the entity to use change triggers.
        /// </summary>
        /// <param name="builder">The entity type builder used for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change trigger.</param>
        /// <returns>The same entity type builder so further calls can be chained.</returns>
        public static EntityTypeBuilder HasChangeTrigger(this EntityTypeBuilder builder, Action<ChangeTriggerOptions>? optionsBuilder = null)
        {
            var trackedEntityType = builder.Metadata;

            builder.HasAnnotation(AnnotationConstants.HasChangeTrigger, true);

            if (optionsBuilder is not null)
            {
                builder.ConfigureChangeTrigger(optionsBuilder);
            }

            return builder;
        }

        /// <summary>
        /// Configures the change trigger for the entity.
        /// </summary>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <param name="optionsBuilder">An action to configure the change trigger.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder ConfigureChangeTrigger(this EntityTypeBuilder builder, Action<ChangeTriggerOptions> optionsBuilder)
        {
            var options = ChangeTriggerOptions.Create(optionsBuilder);

            if (options.TriggerNameFactory != null)
            {
                var triggerNameFormat = TriggerNameFormatHelper.GetTriggerNameFormat(options.TriggerNameFactory, builder.Metadata);
                builder.HasAnnotation(AnnotationConstants.TriggerNameFormat, triggerNameFormat);
            }

            return builder;
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <returns>A builder to further configure the change table.</returns>
        public static ChangeTableBuilder IsChangeTable(this EntityTypeBuilder builder)
        {
            builder.HasAnnotation(AnnotationConstants.IsChangeTable, true);

            return new ChangeTableBuilder(builder);
        }
    }
}