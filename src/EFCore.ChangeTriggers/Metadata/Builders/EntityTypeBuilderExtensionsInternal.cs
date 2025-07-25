﻿using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.ChangeTriggers.Metadata.Builders
{
    internal static class EntityTypeBuilderExtensionsInternal
    {
        public static EntityTypeBuilder HasChangeTriggerInternal(this EntityTypeBuilder builder)
        {
            // https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/breaking-changes#sqlserver-tables-with-triggers
            builder.ToTable(t => t.HasTrigger("ChangeTrigger"));

            // Configure relationship between TTracked and TChange using primary key as foreign key
            var trackedTablePrimaryKey = builder.Metadata.FindPrimaryKey()
                ?? throw new ChangeTriggersConfigurationException(
                    ExceptionStrings.NoPrimaryKeyConfigured(builder.Metadata.DisplayName()));

            builder
                .HasMany(nameof(ITracked<_>.Changes))
                .WithOne(nameof(IHasTrackedEntity<_>.TrackedEntity))
                .HasForeignKey(trackedTablePrimaryKey.Properties.Select(p => p.Name).ToArray())
                .IsTrackedEntityForeignKey();

            return builder;
        }

        public static EntityTypeBuilder IsChangeTableInternal(this EntityTypeBuilder builder)
        {
            builder.Property(nameof(IChange.OperationType))
                .HasColumnName("OperationTypeId")
                .IsOperationTypeProperty();

            builder.Property(nameof(IChange.ChangedAt))
                .IsChangedAtProperty();

            foreach (var foreignKey in builder.Metadata.GetForeignKeys())
            {
                // TODO: Make these optional?
                foreignKey.HasNoCheck(); // Stops cascade delete from removing the change entities if the source entity is deleted
                foreignKey.IsRequired = false; // Forces a LEFT JOIN so change entities can still be queried if the source entity is deleted
            }

            return builder;
        }

        public static EntityTypeBuilder HasChangedByInternal(this EntityTypeBuilder builder, Type changedByClrType)
        {
            const string changedByPropertyName = nameof(IHasChangedBy<_>.ChangedBy);

            builder.EnsureSameClrType(changedByPropertyName, changedByClrType);

            var changedByEntity = builder.Metadata.Model.FindEntityType(changedByClrType);
            if (changedByEntity != null)
            {
                // Configure ChangedBy as a relationship with navigation

                changedByEntity.EnsureSinglePrimaryKey();

                builder
                    .HasOne(changedByClrType, changedByPropertyName)
                    .WithMany()
                    .IsChangedByForeignKey();
            }
            else
            {
                // Configure ChangedBy as a scalar property
                builder.Property(nameof(IHasChangedBy<_>.ChangedBy))
                    .IsChangedByProperty();
            }

            return builder;
        }

        public static EntityTypeBuilder HasChangeSourceInternal(this EntityTypeBuilder builder, Type changeSourceClrType)
        {
            const string changeSourcePropertyName = nameof(IHasChangeSource<_>.ChangeSource);

            builder.EnsureSameClrType(changeSourcePropertyName, changeSourceClrType);

            var changeSourceEntity = builder.Metadata.Model.FindEntityType(changeSourceClrType);
            if (changeSourceEntity != null)
            {
                // Configure ChangeSource as a relationship with navigation
                changeSourceEntity.EnsureSinglePrimaryKey();

                builder
                    .HasOne(changeSourceClrType, changeSourcePropertyName)
                    .WithMany()
                    .IsChangeSourceForeignKey();
            }
            else
            {
                // Configure ChangeSource as a scalar property
                builder.Property(nameof(IHasChangeSource<_>.ChangeSource))
                    .IsChangeSourceProperty();
            }

            return builder;
        }

        public static void EnsureSameClrType(this EntityTypeBuilder builder, string propertyName, Type clrType)
        {
            var property = builder.Metadata.FindProperty(propertyName);
            if (property != null && property.ClrType != clrType)
            {
                throw new ChangeTriggersConfigurationException(ExceptionStrings.PropertyTypeDoesNotMatch(
                    builder.Metadata.DisplayName(), propertyName, property.ClrType.ToString(), clrType.ToString()));
            }

            var navigation = builder.Metadata.FindNavigation(propertyName);
            if (navigation != null && navigation.ClrType != clrType)
            {
                throw new ChangeTriggersConfigurationException(ExceptionStrings.PropertyTypeDoesNotMatch(
                    builder.Metadata.DisplayName(), propertyName, navigation.ClrType.ToString(), clrType.ToString()));
            }
        }
    }
}
