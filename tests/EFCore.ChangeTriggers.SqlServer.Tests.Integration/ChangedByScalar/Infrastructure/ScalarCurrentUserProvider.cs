namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;

internal class ScalarCurrentUserProvider
{
    public string CurrentUser { get; set; }

    public string CurrentUserAsync { get; set; }

    public string MigrationCurrentUser { get; set; }

    public string MigrationCurrentUserAsync { get; set; }
}
