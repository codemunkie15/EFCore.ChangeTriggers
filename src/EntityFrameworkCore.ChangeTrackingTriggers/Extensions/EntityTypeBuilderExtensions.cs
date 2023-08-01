using EntityFrameworkCore.ChangeTrackingTriggers.Abstractions;
using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Exceptions;
using EntityFrameworkCore.ChangeTrackingTriggers.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configures the entity to use change tracking triggers.
        /// </summary>
        /// <typeparam name="TTrackedEntity">The <see cref="ITracked{TChangeType}"/> entity type to configure change tracking triggers for.</typeparam>
        /// <typeparam name="TChangeEntity">The <see cref="IChange{TTracked, TChangeId}"/> entity type that tracks the changes for the <typeparamref name="TTrackedEntity"/>.</typeparam>
        /// <typeparam name="TChangeId">The <typeparamref name="TChangeEntity"/> change identifier type.</typeparam>
        /// <param name="builder">The entity type builder used for configuration.</param>
        /// <param name="optionsBuilder">An optional action to configure the change tracking trigger.</param>
        /// <returns>The same entity type builder so further calls can be chained.</returns>
        /// <exception cref="ChangeTrackingTriggersConfigurationException"></exception>
        public static EntityTypeBuilder<TTrackedEntity> HasChangeTrackingTrigger<TTrackedEntity, TChangeEntity>(
            this EntityTypeBuilder<TTrackedEntity> builder,
            Action<ChangeTrackingTriggerOptions>? optionsBuilder = null)
            where TTrackedEntity : class, ITracked<TChangeEntity>
            where TChangeEntity: class, IHasTrackedEntity<TTrackedEntity>
        {
            var options = ChangeTrackingTriggerOptions.Create(optionsBuilder);

            var trackedEntityType = builder.Metadata.Model.FindEntityType(typeof(TTrackedEntity))!;
            var changeEntityType = builder.Metadata.Model.FindEntityType(typeof(TChangeEntity))!;

            builder.HasAnnotation(AnnotationConstants.UseChangeTrackingTriggers, true);
            builder.HasAnnotation(AnnotationConstants.ChangeEntityTypeName, changeEntityType!.Name);
            changeEntityType.AddAnnotation(AnnotationConstants.TrackedEntityTypeName, trackedEntityType!.Name);

            // https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes#sqlserver-tables-with-triggers
            builder.ToTable(t => t.HasTrigger("ChangeTrackingTrigger"));

            // Configure relationship between TTracked and TChange using primary key as foreign key
            var trackedTablePrimaryKey = trackedEntityType.FindPrimaryKey();

            if (trackedTablePrimaryKey == null)
            {
                throw new ChangeTrackingTriggersConfigurationException(
                    $"Tracked entity type '{trackedEntityType.Name}' must have a primary key configured to use change tracking triggers.");
            }

            builder
                .HasMany(e => e.Changes)
                .WithOne(e => e.TrackedEntity)
                .HasForeignKey(trackedTablePrimaryKey.Properties.Select(p => p.Name).ToArray())
                .IsRequired(false) // FK column must be nullable to force left join as source record may have been deleted
                .HasNoCheck(); // Must be NOCHECK so the FK reference can exist even if the source record is deleted

            if (optionsBuilder is not null)
            {
                builder.ConfigureChangeTrackingTrigger(optionsBuilder);
            }

            return builder;
        }

        /// <summary>
        /// Configures the change tracking trigger for the entity.
        /// </summary>
        /// <typeparam name="TTrackedEntity">The tracked entity to configure.</typeparam>
        /// <param name="builder">The entity type builder to use for configuration.</param>
        /// <param name="optionsBuilder">An action to configure the change tracking trigger.</param>
        /// <returns>The same entity type builder so that further calls can be chained.</returns>
        public static EntityTypeBuilder<TTrackedEntity> ConfigureChangeTrackingTrigger<TTrackedEntity>(
            this EntityTypeBuilder<TTrackedEntity> builder,
            Action<ChangeTrackingTriggerOptions> optionsBuilder)
            where TTrackedEntity : class
        {
            var options = ChangeTrackingTriggerOptions.Create(optionsBuilder);

            if (options.TriggerNameFactory != null)
            {
                var trackedEntityType = builder.Metadata.Model.FindEntityType(typeof(TTrackedEntity))!;
                var triggerNameFormat = TriggerNameFormatHelper.GetTriggerNameFormat(options.TriggerNameFactory, trackedEntityType);
                builder.HasAnnotation(AnnotationConstants.TriggerNameFormat, triggerNameFormat);
            }

            return builder;
        }

        public static EntityTypeBuilder<TChangeEntity> IsChangeTrackingTable<TChangeEntity, TChangeId, TChangedBy, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTrackingTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>, IHasChangedBy<TChangedBy>, IHasChangeSource<TChangeSource>
        {
            return builder
                .IsChangeTrackingTable<TChangeEntity, TChangeId>()
                .HasChangedBy<TChangeEntity, TChangedBy>()
                .HasChangeSource(optionsBuilder);
        }

        public static EntityTypeBuilder<TChangeEntity> IsChangeTrackingTable<TChangeEntity, TChangeId>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>
        {
            builder.HasKey(e => e.ChangeId);

            builder.Property(e => e.OperationType)
                .HasColumnName("OperationTypeId")
                .IsOperationTypeProperty();

            builder.Property(e => e.ChangedAt)
                .IsChangedAtProperty();

            return builder;
        }

        public static EntityTypeBuilder<TChangeEntity> IsChangeTrackingTable<TChangeEntity, TChangeId, TChangedBy>(
            this EntityTypeBuilder<TChangeEntity> builder)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>, IHasChangedBy<TChangedBy>
        {
            return builder
                .IsChangeTrackingTable<TChangeEntity, TChangeId>()
                .HasChangedBy<TChangeEntity, TChangedBy>();
        }

        public static EntityTypeBuilder<TChangeEntity> IsChangeTrackingTable<TChangeEntity, TChangeId, TChangeSource>(
            this EntityTypeBuilder<TChangeEntity> builder,
            Action<ChangeTrackingTableOptions<TChangeSource>>? optionsBuilder = null)
            where TChangeEntity : class, IChange, IHasChangeId<TChangeId>, IHasChangeSource<TChangeSource>
        {
            return builder
                .IsChangeTrackingTable<TChangeEntity, TChangeId>()
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
                    .IsChangedByForeignKey() // Used to find the column type for the trigger
                    .IsRequired(false) // FK column must be nullable to force left join as ChangedBy record may have been deleted
                    .HasNoCheck(); // Must be NOCHECK so the FK reference can exist even if the ChangedBy record has been deleted
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
            Action<ChangeTrackingTableOptions<TChangeSource>>? optionsBuilder = null)
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