namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByScalar;

public class ScalarCurrentUserProvider
{
    public SyncOrAsyncValue<string> CurrentUser { get; } = new();

    public SyncOrAsyncValue<string> MigrationCurrentUser { get; } = new();
}
