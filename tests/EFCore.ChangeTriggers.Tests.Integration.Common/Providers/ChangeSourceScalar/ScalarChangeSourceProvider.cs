using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;

public class ScalarChangeSourceProvider
{
    public SyncOrAsyncValue<ChangeSourceType> CurrentChangeSource { get; } = new();

    public SyncOrAsyncValue<ChangeSourceType> MigrationChangeSource { get; } = new();
}
