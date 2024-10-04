using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Infrastructure;

internal class ChangedByEntityCurrentUserProvider
{
    public ChangedByEntityUser CurrentUser { get; set; }
}
