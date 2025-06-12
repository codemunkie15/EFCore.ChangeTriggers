using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;

public class EntityCurrentUserProvider
{
    public SyncOrAsyncValue<ChangedByEntityUser> CurrentUser { get; } = new();

    public SyncOrAsyncValue<ChangedByEntityUser> MigrationCurrentUser { get; } = new();
}