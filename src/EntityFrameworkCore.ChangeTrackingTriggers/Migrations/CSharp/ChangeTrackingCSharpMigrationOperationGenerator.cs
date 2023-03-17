﻿using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.CSharp
{
    internal class ChangeTrackingCSharpMigrationOperationGenerator : CSharpMigrationOperationGenerator
    {
        public ChangeTrackingCSharpMigrationOperationGenerator(CSharpMigrationOperationGeneratorDependencies dependencies)
            : base(dependencies)
        {
        }

        protected override void Generate(MigrationOperation operation, IndentedStringBuilder builder)
        {
            if (operation is CreateChangeTrackingTriggerOperation createChangeTrackingTriggerOperation)
            {
                Generate(createChangeTrackingTriggerOperation, builder);
            }
            else if (operation is DropChangeTrackingTriggerOperation dropChangeTrackingTriggerOperation)
            {
                Generate(dropChangeTrackingTriggerOperation, builder);
            }
            else if (operation is NoCheckConstraintOperation noCheckConstraintOperation)
            {
                Generate(noCheckConstraintOperation, builder);
            }
            else
            {
                base.Generate(operation, builder);
            }
        }

        protected virtual void Generate(CreateChangeTrackingTriggerOperation operation, IndentedStringBuilder builder)
        {
            builder
                .AppendLine(".CreateChangeTrackingTrigger(");

            using (builder.Indent())
            {
                builder
                    .Append("trackedTableName: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.TrackedTableName))
                    .AppendLine(",")
                    .Append("changeTableName: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.ChangeTableName))
                    .AppendLine(",")
                    .Append("triggerName: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.TriggerName))
                    .AppendLine(",")
                    .Append("trackedTablePrimaryKeyColumns: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.TrackedTablePrimaryKeyColumns.ToArray()))
                    .AppendLine(",")
                    .Append("changeTableDataColumns: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.ChangeTableDataColumns.ToArray()))
                    .AppendLine(",")
                    .Append("operationTypeColumn: ")
                    .Append($"new {nameof(ChangeContextColumn)}(")
                    .Append(Dependencies.CSharpHelper.Arguments(new[] { operation.OperationTypeColumn.Name, operation.OperationTypeColumn.Type! }))
                    .AppendLine("),")
                    .Append("changedAtColumn: ")
                    .Append($"new {nameof(ChangeContextColumn)}(")
                    .Append(Dependencies.CSharpHelper.Arguments(new[] { operation.ChangedAtColumn.Name }))
                    .Append(")");

                if (operation.ChangeSourceColumn != null)
                {
                    builder
                        .AppendLine(",")
                        .Append("changeSourceColumn: ")
                        .Append($"new {nameof(ChangeContextColumn)}(")
                        .Append(Dependencies.CSharpHelper.Arguments(new[] { operation.ChangeSourceColumn.Name, operation.ChangeSourceColumn.Type! }))
                        .Append(")");
                }

                if (operation.ChangedByColumn != null)
                {
                    builder
                        .AppendLine(",")
                        .Append("changedByColumn: ")
                        .Append($"new {nameof(ChangeContextColumn)}(")
                        .Append(Dependencies.CSharpHelper.Arguments(new[] { operation.ChangedByColumn.Name, operation.ChangedByColumn.Type! }))
                        .Append(")");
                }
            }

            builder.Append(")");
        }

        protected virtual void Generate(DropChangeTrackingTriggerOperation operation, IndentedStringBuilder builder)
        {
            builder
                .AppendLine(".DropChangeTrackingTrigger(");

            using (builder.Indent())
            {
                builder
                    .Append("triggerName: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.TriggerName));
            }

            builder.Append(")");
        }

        protected virtual void Generate(NoCheckConstraintOperation operation, IndentedStringBuilder builder)
        {
            builder.AppendLine(".AddNoCheckConstraint(");

            using (builder.Indent())
            {
                if (operation.Schema != null)
                {
                    builder
                    .Append("schema: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.Schema))
                    .AppendLine(",");
                }

                builder
                    .Append("table: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.Table))
                    .AppendLine(",")
                    .Append("constraint: ")
                    .Append(Dependencies.CSharpHelper.Literal(operation.Constraint))
                    .Append(")");
            }
        }
    }
}
