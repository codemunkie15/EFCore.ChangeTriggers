using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Sql
{
    internal interface IChangeTrackingTriggerSqlGenerator
    {
        void Generate(CreateChangeTrackingTriggerOperation operation, MigrationCommandListBuilder builder);
    }
}
