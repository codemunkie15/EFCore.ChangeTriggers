using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

internal class EntityCurrentUserProvider
{
    public Guid InstanceId => Guid.NewGuid();

    public ChangedByEntityUser CurrentUser { get; set; }

    public ChangedByEntityUser CurrentUserAsync { get; set; }
}
