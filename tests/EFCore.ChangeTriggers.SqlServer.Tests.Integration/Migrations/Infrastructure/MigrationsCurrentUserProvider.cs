using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure;

internal class MigrationsCurrentUserProvider
{
    public MigrationsUser CurrentUser { get; set; }
}
