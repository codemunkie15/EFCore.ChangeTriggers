using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

internal class CurrentUserProvider
{
    public Guid InstanceId = Guid.NewGuid();

    public EntityUser CurrentUser { get; set; }

    public CurrentUserProvider(EntityUser currentUser)
    {
        CurrentUser = currentUser;
    }
}
