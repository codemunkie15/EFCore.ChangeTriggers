namespace EFCore.ChangeTriggers.MySql.Templates
{
    internal record ChangeConfig(int OperationTypeId, string tableName, string TableAlias);
}
