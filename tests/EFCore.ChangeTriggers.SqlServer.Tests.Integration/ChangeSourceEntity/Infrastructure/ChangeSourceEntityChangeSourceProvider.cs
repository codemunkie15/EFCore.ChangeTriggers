using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Infrastructure;

internal class ChangeSourceEntityChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }
}
