using EntityFrameworkCore.ChangeTrackingTriggers.Configuration;
using EntityFrameworkCore.ChangeTrackingTriggers.Constants;
using EntityFrameworkCore.ChangeTrackingTriggers.Extensions;
using EntityFrameworkCore.ChangeTrackingTriggers.Helpers;
using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using EntityFrameworkCore.ChangeTrackingTriggers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations
{
    public class ChangeTrackingTriggersMigrationsModelDiffer : MigrationsModelDiffer
    {
        private readonly ChangeTrackingTriggersOptions changeTrackingTriggersOptions;

        public ChangeTrackingTriggersMigrationsModelDiffer(
            ChangeTrackingTriggersOptions changeTrackingTriggersOptions,
            IRelationalTypeMappingSource typeMappingSource,
            IMigrationsAnnotationProvider migrationsAnnotationProvider,
            IRowIdentityMapFactory rowIdentityMapFactory,
            CommandBatchPreparerDependencies commandBatchPreparerDependencies)
            : base(typeMappingSource, migrationsAnnotationProvider, rowIdentityMapFactory, commandBatchPreparerDependencies)
        {
            this.changeTrackingTriggersOptions = changeTrackingTriggersOptions;
        }

        protected override IEnumerable<MigrationOperation> Diff(IRelationalModel? source, IRelationalModel? target, DiffContext diffContext)
        {
            var sourceChangeTracked = GetChangeTrackedEntites(source);
            var targetChangeTracked = GetChangeTrackedEntites(target);

            return base.Diff(source, target, diffContext)
                .Concat(Diff(sourceChangeTracked, targetChangeTracked, diffContext));
        }

        protected override IReadOnlyList<MigrationOperation> Sort(IEnumerable<MigrationOperation> operations, DiffContext diffContext)
        {
            var dataOperations = new List<Type> { typeof(InsertDataOperation), typeof(UpdateDataOperation), typeof(DeleteDataOperation) };
            var sortedOperations = new ObservableCollection<MigrationOperation>(base.Sort(operations, diffContext));

            // Move NOCHECK constraint operations before data operations
            var firstDataOperationIndex = sortedOperations.ToList().FindIndex(o => dataOperations.Any(dataOp => o.GetType() == dataOp));
            if (firstDataOperationIndex > -1)
            {
                MoveOperations(sortedOperations, sortedOperations.OfType<NoCheckConstraintOperation>().ToList(), firstDataOperationIndex);
            }

            // Move CreateChangeTrackingTriggerOperation before data operations
            firstDataOperationIndex = sortedOperations.ToList().FindIndex(o => dataOperations.Any(dataOp => o.GetType() == dataOp));
            if (firstDataOperationIndex > -1)
            {
                MoveOperations(sortedOperations, sortedOperations.OfType<CreateChangeTrackingTriggerOperation>().ToList(), firstDataOperationIndex);
            }

            // Move DropChangeTrackingTriggerOperation before DropTableOperation
            var firstDropTableOperationIndex = sortedOperations.ToList().FindIndex(o => o.GetType() == typeof(DropTableOperation));
            if (firstDropTableOperationIndex > -1)
            {
                MoveOperations(sortedOperations, sortedOperations.OfType<DropChangeTrackingTriggerOperation>().ToList(), firstDropTableOperationIndex);
            }

            return sortedOperations;
        }

        protected override IEnumerable<MigrationOperation> Add(IForeignKeyConstraint target, DiffContext diffContext)
        {
            return base.Add(target, diffContext)
                .Concat(AddNoCheckConstraints(target, diffContext));
        }

        private IEnumerable<NoCheckConstraintOperation> AddNoCheckConstraints(IForeignKeyConstraint target, DiffContext diffContext)
        {
            foreach (var foreignKey in target.MappedForeignKeys)
            {
                if (foreignKey.HasNoCheck())
                {
                    yield return new NoCheckConstraintOperation
                    {
                        Schema = target.Table.Schema,
                        Table = target.Table.Name,
                        Constraint = target.Name
                    };
                }
            }
        }

        private IEnumerable<MigrationOperation> Diff(IEnumerable<ChangeTrackedEntity> sources, IEnumerable<ChangeTrackedEntity> targets, DiffContext diffContext)
            => DiffCollection(
                sources,
                targets,
                diffContext,
                Diff,
                Add,
                Remove,
                Compare);

        private IEnumerable<MigrationOperation> Diff(ChangeTrackedEntity source, ChangeTrackedEntity target, DiffContext diffContext)
        {
            if (source.Trigger == target.Trigger)
            {
                yield break; // No changes needed
            }

            // Just drop the trigger and rebuild it
            yield return GenerateDropChangeTrackingTriggerOperation(source);
            yield return GenerateCreateChangeTrackingTriggerOperation(target);
        }

        private IEnumerable<MigrationOperation> Add(ChangeTrackedEntity target, DiffContext diffContext)
        {
            yield return GenerateCreateChangeTrackingTriggerOperation(target);
        }

        private IEnumerable<MigrationOperation> Remove(ChangeTrackedEntity source, DiffContext diffContext)
        {
            yield return GenerateDropChangeTrackingTriggerOperation(source);
        }

        private bool Compare(ChangeTrackedEntity source, ChangeTrackedEntity target, DiffContext diffContext)
        {
            // Are the source and target for the same trigger?
            return string.Equals(source.TrackedEntityType.Name, target.TrackedEntityType.Name);
        }

        private IEnumerable<ChangeTrackedEntity> GetChangeTrackedEntites(IRelationalModel? model)
        {
            var trackedEntityTypes = model?.Model.GetEntityTypes().Where(e => e.IsChangeTracked()) ?? Enumerable.Empty<IEntityType>();

            foreach (var trackedEntityType in trackedEntityTypes)
            {
                yield return GetChangeTrackedEntity(trackedEntityType);
            }
        }

        private ChangeTrackedEntity GetChangeTrackedEntity(IEntityType trackedEntityType)
        {
            var changeEntityType = trackedEntityType.GetChangeEntityType();
            var changeTable = changeEntityType.GetTableMappings().First().Table;

            var changeTableProperties = changeEntityType.GetProperties().ToList();
            var changeTableDataColumns = changeTableProperties.Where(p => !p.IsPrimaryKey() && !p.IsChangeContextProperty());

            var trackedPrimaryKeyProperties = trackedEntityType.GetProperties().Where(p => p.IsPrimaryKey());

            var operationTypeProperty = changeTableProperties.Single(p => p.IsOperationTypeProperty());
            var changedAtProperty = changeTableProperties.Single(p => p.IsChangedAtProperty());

            var changeSourceProperty = changeTableProperties.SingleOrDefault(p => p.IsChangeSourceProperty());
            var changedByProperty = changeTableProperties.SingleOrDefault(p => p.IsChangedByProperty());

            return new ChangeTrackedEntity
            {
                TrackedEntityType = trackedEntityType,
                ChangeEntityType = changeEntityType,
                Trigger = new ChangeTrackingTrigger
                {
                    Name = GetTriggerName(trackedEntityType),
                    ChangeTableDataColumns = changeTableDataColumns.Select(c => c.Name),
                    TrackedTablePrimaryKeyColumns = trackedPrimaryKeyProperties.Select(p => p.GetColumnName()),
                    OperationTypeColumn = operationTypeProperty.AsChangeContextColumn()!,
                    ChangeSourceColumn = changeSourceProperty?.AsChangeContextColumn(),
                    ChangedByColumn = changedByProperty?.AsChangeContextColumn(),
                    ChangedAtColumn = changedAtProperty.AsChangeContextColumn()!
                }
            };
        }

        private CreateChangeTrackingTriggerOperation GenerateCreateChangeTrackingTriggerOperation(ChangeTrackedEntity entity)
        {
            return new CreateChangeTrackingTriggerOperation
            {
                TrackedTableName = entity.TrackedEntityType.GetTableName()!,
                ChangeTableName = entity.ChangeEntityType!.GetTableName()!,
                TriggerName = entity.Trigger.Name,
                TrackedTablePrimaryKeyColumns = entity.Trigger.TrackedTablePrimaryKeyColumns,
                ChangeTableDataColumns = entity.Trigger.ChangeTableDataColumns,
                OperationTypeColumn = entity.Trigger.OperationTypeColumn,
                ChangeSourceColumn = entity.Trigger.ChangeSourceColumn,
                ChangedByColumn = entity.Trigger.ChangedByColumn,
                ChangedAtColumn = entity.Trigger.ChangedAtColumn
            };
        }

        private DropChangeTrackingTriggerOperation GenerateDropChangeTrackingTriggerOperation(ChangeTrackedEntity entity)
        {
            return new DropChangeTrackingTriggerOperation
            {
                TriggerName = entity.Trigger.Name
            };
        }

        private void MoveOperations(ObservableCollection<MigrationOperation> sortedOperations, IEnumerable<MigrationOperation> operationsToMove, int newStartingIndex)
        {
            var offset = 0;
            foreach (var operation in operationsToMove)
            {
                var currentIndex = sortedOperations.IndexOf(operation);
                sortedOperations.Move(currentIndex, newStartingIndex + offset);
                offset++;
            }
        }

        private string GetTriggerName(IEntityType trackedEntityType)
        {
            // Ordered lowest to highest presidence

            var triggerNameFormat = ChangeTrackingTriggerDefaults.TriggerNameFormat;

            if (changeTrackingTriggersOptions.TriggerNameFactory != null)
            {
                triggerNameFormat = TriggerNameFormatHelper.GetTriggerNameFormat(changeTrackingTriggersOptions.TriggerNameFactory);
            }

            var entityTypeFormat = trackedEntityType.GetTriggerNameFormat();
            if (entityTypeFormat != null)
            {
                triggerNameFormat = entityTypeFormat;
            }

            return string.Format(triggerNameFormat, trackedEntityType.GetTableName());
        }
    }
}