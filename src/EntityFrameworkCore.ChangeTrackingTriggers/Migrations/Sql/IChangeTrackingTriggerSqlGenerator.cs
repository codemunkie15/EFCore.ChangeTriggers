using EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkCore.ChangeTrackingTriggers.Migrations.Sql
{
    internal interface ICreateChangeTrackingTriggerSqlGenerator
    {
        void Generate(CreateChangeTrackingTriggerOperation operation, MigrationCommandListBuilder builder);
    }
}
