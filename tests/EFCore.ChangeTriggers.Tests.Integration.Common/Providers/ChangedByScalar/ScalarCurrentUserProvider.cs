namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;

public class ScalarCurrentUserProvider
{
    public string CurrentUser { get; set; }

    public string CurrentUserAsync { get; set; }

    public string MigrationCurrentUser { get; set; }

    public string MigrationCurrentUserAsync { get; set; }
}
