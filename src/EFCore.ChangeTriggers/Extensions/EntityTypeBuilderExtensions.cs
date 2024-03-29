﻿using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Configuration;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Exceptions;
using EFCore.ChangeTriggers.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configures the entity to use change triggers.
        /// </summary>
        /// <typeparam name="TTrackedEntity">The <see cref="ITracked{TChangeType}"/> entity type to configure change triggers for.</typeparam>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked, TChangeId}"/> entity type that tracks the changes for the <typeparamref name="TTrackedEntity"/>.</typeparam>
        /// <param name="builder">The entity type builder used for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change trigger.</param>
        /// <returns>The same entity type builder so further calls can be chained.</returns>
        /// <exception cref="ChangeTriggersConfigurationException"></exception>
        public static EntityTypeBuilder<TTrackedEntity> HasChangeTrigger<TTrackedEntity, TChangeEntity>(
            this EntityTypeBuilder<TTrackedEntity> builder,
            Action<ChangeTriggerOptions>? optionsBuilder = null)
            where TTrackedEntity : class, ITracked<TChangeEntity>
            where TChangeEntity : class, IHasTrackedEntity<TTrackedEntity>
        {
            var options = ChangeTriggerOptions.Create(optionsBuilder);

            var trackedEntityType = builder.Metadata;
            var changeEntityType = builder.Metadata.Model.FindEntityType(typeof(TChangeEntity))!;

            builder.HasAnnotation(AnnotationConstants.UseChangeTriggers, true);
            builder.HasAnnotation(AnnotationConstants.ChangeEntityTypeName, changeEntityType!.Name);
            changeEntityType.AddAnnotation(AnnotationConstants.TrackedEntityTypeName, trackedEntityType!.Name);

            // https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes#sqlserver-tables-with-triggers
            builder.ToTable(t => t.HasTrigger("ChangeTrigger"));

            // Configure relationship between TTracked and TChange using primary key as foreign key
            var trackedTablePrimaryKey = trackedEntityType.FindPrimaryKey();

            if (trackedTablePrimaryKey == null)
            {
                throw new ChangeTriggersConfigurationException(
                    $"Tracked entity type '{trackedEntityType.Name}' must have a primary key configured to use change triggers.");
            }

            builder
                .HasMany(e => e.Changes)
                .WithOne(e => e.TrackedEntity)
                .HasForeignKey(trackedTablePrimaryKey.Properties.Select(p => p.Name).ToArray());

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
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked, TChangeId}"/> entity type to configure.</typeparam>
        /// <typeparam name="TChangeId">The <typeparamref name="TChangeEntity"/> change identifier type.</typeparam>
        /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
        /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change table.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity, TChangeId, TChangedBy, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
        {
            return builder
                .IsChangeTable<TChangeEntity, TChangeId>()
                .HasChangedBy<TChangeEntity, TChangedBy>()
                .HasChangeSource(optionsBuilder);
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked, TChangeId}"/> entity type to configure.</typeparam>
        /// <typeparam name="TChangeId">The <typeparamref name="TChangeEntity"/> change identifier type.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity, TChangeId>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>
        {
            builder.HasKey(e => e.ChangeId);

            builder.Property(e => e.OperationType)
                .HasColumnName("OperationTypeId")
                .IsOperationTypeProperty();

            builder.Property(e => e.ChangedAt)
                .IsChangedAtProperty();

            foreach (var foreignKey in builder.Metadata.GetForeignKeys())
            {
                foreignKey.HasNoCheck(); // Stops cascade delete from removing the change entities if the source entity is deleted
                foreignKey.IsRequired = false; // Forces a LEFT JOIN so change entities can still be queried if the source entity is deleted
            }

            return builder;
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked, TChangeId}"/> entity type to configure.</typeparam>
        /// <typeparam name="TChangeId">The <typeparamref name="TChangeEntity"/> change identifier type.</typeparam>
        /// <typeparam name="TChangedBy">The type that represents who a change was made by.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity, TChangeId, TChangedBy>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>, IHasChangedBy<TChangedBy>
        {
            return builder
                .IsChangeTable<TChangeEntity, TChangeId>()
                .HasChangedBy<TChangeEntity, TChangedBy>();
        }

        /// <summary>
        /// Configures the entity as a change table.
        /// </summary>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked, TChangeId}"/> entity type to configure.</typeparam>
        /// <typeparam name="TChangeId">The <typeparamref name="TChangeEntity"/> change identifier type.</typeparam>
        /// <typeparam name="TChangeSource">The type that represents where a change originated from.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change table.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TChangeEntity> IsChangeTable<TChangeEntity, TChangeId, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>, IHasChangeSource<TChangeSource>
        {
            return builder
                .IsChangeTable<TChangeEntity, TChangeId>()
                .HasChangeSource(optionsBuilder);
        }

        private static EntityTypeBuilder<TChangeEntity> HasChangedBy<TChangeEntity, TChangedBy>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange, IHasChangedBy<TChangedBy>
        {
            var changedByEntity = builder.Metadata.Model.FindEntityType(typeof(TChangedBy));
            if (changedByEntity != null)
            {
                // Configure ChangedBy as a relationship with navigation

                changedByEntity.EnsureSinglePrimaryKey();

                builder
                    .HasOne(typeof(TChangedBy), nameof(IHasChangedBy<TChangedBy>.ChangedBy)) // Can't use a navigation expression because TChangedBy may not be a class
                    .WithMany()
                    .IsChangedByForeignKey(); // Used to find the column type for the trigger
            }
            else
            {
                // Configure ChangedBy as a scalar property
                builder.Property(e => e.ChangedBy)
                    .IsChangedByProperty();
            }

            return builder;
        }

        private static EntityTypeBuilder<TChangeEntity> HasChangeSource<TChangeEntity, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangeSource<TChangeSource>
        {
            if (builder.Metadata.Model.FindEntityType(typeof(TChangeSource)) != null)
            {
                // Configure ChangeSource as a foreign key with navigation
                builder
                    .HasOne(typeof(TChangeSource), nameof(IHasChangeSource<TChangeSource>.ChangeSource)) // Can't use a navigation expression because TChangeSource may not be a class
                    .WithMany()
                    .IsChangeSourceForeignKey();
            }
            else
            {
                // Configure ChangedBy as a scalar property
                builder.Property(e => e.ChangeSource)
                .IsChangeSourceProperty();
            }

            return builder;
        }
    }
}