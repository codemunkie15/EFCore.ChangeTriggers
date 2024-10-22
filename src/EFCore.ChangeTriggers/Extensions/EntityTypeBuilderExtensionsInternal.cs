using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Constants;
using EFCore.ChangeTriggers.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Extensions
{
    internal static class EntityTypeBuilderExtensionsInternal
    {
        public static EntityTypeBuilder HasChangeTriggerInternal(
            this EntityTypeBuilder builder)
        {
            // https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes#sqlserver-tables-with-triggers
            builder.ToTable(t => t.HasTrigger("ChangeTrigger"));

            // Configure relationship between TTracked and TChange using primary key as foreign key
            var trackedTablePrimaryKey = builder.Metadata.FindPrimaryKey()
                ?? throw new ChangeTriggersConfigurationException(
                    $"Tracked entity type '{builder.Metadata.Name}' must have a primary key configured to use change triggers.");

            builder
                .HasMany(nameof(ITracked<object>.Changes))
                .WithOne(nameof(IHasTrackedEntity<object>.TrackedEntity))
                .HasForeignKey(trackedTablePrimaryKey.Properties.Select(p => p.Name).ToArray())
                .IsTrackedEntityForeignKey();

            return builder;
        }

        public static EntityTypeBuilder IsChangeTableInternal(
            this EntityTypeBuilder builder)
        {
            builder.Property(nameof(IChange.OperationType))
                .HasColumnName("OperationTypeId")
                .IsOperationTypeProperty();

            builder.Property(nameof(IChange.ChangedAt))
                .IsChangedAtProperty();

            foreach (var foreignKey in builder.Metadata.GetForeignKeys())
            {
                foreignKey.HasNoCheck(); // Stops cascade delete from removing the change entities if the source entity is deleted
                foreignKey.IsRequired = false; // Forces a LEFT JOIN so change entities can still be queried if the source entity is deleted
            }

            return builder;
        }

        public static EntityTypeBuilder HasChangedByInternal(
            this EntityTypeBuilder builder)
        {
            var changedByClrType = builder.Metadata.GetChangedByClrTypeName();
            var changedByEntity = builder.Metadata.Model.FindEntityType(changedByClrType);
            if (changedByEntity != null)
            {
                // Configure ChangedBy as a relationship with navigation

                changedByEntity.EnsureSinglePrimaryKey();

                builder
                    .HasOne(changedByClrType, nameof(IHasChangedBy<object>.ChangedBy))
                    .WithMany()
                    .IsChangedByForeignKey();
            }
            else
            {
                // Configure ChangedBy as a scalar property
                builder.Property(nameof(IHasChangedBy<object>.ChangedBy))
                    .IsChangedByProperty();
            }

            return builder;
        }

        public static EntityTypeBuilder HasChangeSourceInternal(
            this EntityTypeBuilder builder)
        {
            var changeSourceClrType = builder.Metadata.GetChangeSourceClrTypeName();
            var changeSourceEntity = builder.Metadata.Model.FindEntityType(changeSourceClrType);
            if (changeSourceEntity != null)
            {
                // Configure ChangeSource as a relationship with navigation

                changeSourceEntity.EnsureSinglePrimaryKey();

                builder
                    .HasOne(changeSourceClrType, nameof(IHasChangeSource<object>.ChangeSource))
                    .WithMany()
                    .IsChangeSourceForeignKey();
            }
            else
            {
                // Configure ChangeSource as a scalar property
                builder.Property(nameof(IHasChangeSource<object>.ChangeSource))
                    .IsChangeSourceProperty();
            }

            return builder;
        }
    }
}
