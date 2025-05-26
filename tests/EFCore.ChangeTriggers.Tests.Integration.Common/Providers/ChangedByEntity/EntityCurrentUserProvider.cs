using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangedByEntity;

public class EntityCurrentUserProvider
{
    public ChangedByEntityUser CurrentUser { get; set; }

    public ChangedByEntityUser CurrentUserAsync { get; set; }

    public ChangedByEntityUser MigrationCurrentUser { get; set; }

    public ChangedByEntityUser MigrationCurrentUserAsync { get; set; }
}
