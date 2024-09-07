namespace EFCore.ChangeTriggers.SqlServer.Templates
{
    internal record ChangeConfig(int OperationTypeId, string tableName, string TableAlias);
}
