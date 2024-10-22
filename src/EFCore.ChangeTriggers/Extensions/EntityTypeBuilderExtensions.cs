using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Configuration;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configures the entity to use change triggers.
        /// </summary>
        /// <typeparam name="TTrackedEntity">The <see cref="ITracked{TChangeType}"/> entity type to configure change triggers for.</typeparam>
        /// <param name="builder">The entity type builder used for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change trigger.</param>
        /// <returns>The same entity type builder so further calls can be chained.</returns>
        public static EntityTypeBuilder<TTrackedEntity> HasChangeTrigger<TTrackedEntity>(
            this EntityTypeBuilder<TTrackedEntity> builder,
            Action<ChangeTriggerOptions>? optionsBuilder = null)
            where TTrackedEntity : class
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
        /// <typeparam name="TTrackedEntity">The tracked entity to configure.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <param name="optionsBuilder">An action to configure the change trigger.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TTrackedEntity> ConfigureChangeTrigger<TTrackedEntity>(
            this EntityTypeBuilder<TTrackedEntity> builder,
            Action<ChangeTriggerOptions> optionsBuilder)
            where TTrackedEntity : class
        {
            var options = ChangeTriggerOptions.Create(optionsBuilder);

            if (options.TriggerNameFactory != null)
            {
                var trackedEntityType = builder.Metadata.Model.FindEntityType(typeof(TTrackedEntity))!;
                var triggerNameFormat = TriggerNameFormatHelper.GetTriggerNameFormat(options.TriggerNameFactory, trackedEntityType);
                builder.HasAnnotation(AnnotationConstants.TriggerNameFormat, triggerNameFormat);
            }

            return builder;
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked}"/> entity type to configure.</typeparam>
        /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
        /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change table.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity, TChangedBy, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
        {
            return builder
                .IsChangeTable()
                .HasChangedBy<TChangeEntity, TChangedBy>()
                .HasChangeSource(optionsBuilder);
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked}"/> entity type to configure.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange
        {
            builder.HasAnnotation(AnnotationConstants.IsChangeTable, true);

            return builder;
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked}"/> entity type to configure.</typeparam>
        /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity, TChangedBy>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange, IHasChangedBy<TChangedBy>
        {
            return builder
                .IsChangeTable()
                .HasChangedBy<TChangeEntity, TChangedBy>();
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked}"/> entity type to configure.</typeparam>
        /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change table.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangeSource<TChangeSource>
        {
            return builder
                .IsChangeTable()
                .HasChangeSource(optionsBuilder);
        }

        private static EntityTypeBuilder<TChangeEntity> HasChangedBy<TChangeEntity, TChangedBy>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange, IHasChangedBy<TChangedBy>
        {
            builder.HasAnnotation(AnnotationConstants.ChangedByClrTypeName, typeof(TChangedBy).FullName!);
            return builder;
        }

        private static EntityTypeBuilder<TChangeEntity> HasChangeSource<TChangeEntity, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangeSource<TChangeSource>
        {
            builder.HasAnnotation(AnnotationConstants.ChangeSourceClrTypeName, typeof(TChangeSource).FullName!);
            return builder;
        }
    }
}