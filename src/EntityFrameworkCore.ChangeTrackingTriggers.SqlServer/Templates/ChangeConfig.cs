namespace EntityFrameworkCore.ChangeTrackingTriggers.SqlServer.Templates
{
    internal record ChangeConfig(int operationTypeId, string tableName, string tableAlias);
}
