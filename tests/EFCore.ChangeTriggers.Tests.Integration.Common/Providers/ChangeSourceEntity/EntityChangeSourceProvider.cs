using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;

public class EntityChangeSourceProvider
{
    public SyncOrAsyncValue<ChangeSource> CurrentChangeSource { get; } = new();

    public SyncOrAsyncValue<ChangeSource> MigrationChangeSource { get; } = new();
}
