using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Infrastructure;

internal class MigrationsCurrentChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }
}
