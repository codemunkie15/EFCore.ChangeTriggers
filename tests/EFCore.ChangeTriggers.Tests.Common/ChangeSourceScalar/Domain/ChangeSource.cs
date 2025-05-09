namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain
{
    public enum ChangeSource
    {
        None = 0,
        Migrations,
        Tests,
        WebApi,
        Console,
        Sql,
        Mobile,
        PublicApi,
        EmailService,
        DataRetentionService,
        Maintenance
    }
}
