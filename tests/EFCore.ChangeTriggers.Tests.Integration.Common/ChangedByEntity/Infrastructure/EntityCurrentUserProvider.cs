using EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Infrastructure;

public class EntityCurrentUserProvider
{
    public ChangedByEntityUser CurrentUser { get; set; }

    public ChangedByEntityUser CurrentUserAsync { get; set; }

    public ChangedByEntityUser MigrationCurrentUser { get; set; }

    public ChangedByEntityUser MigrationCurrentUserAsync { get; set; }
}
