﻿namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar
{
    public enum ChangeSourceType
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
