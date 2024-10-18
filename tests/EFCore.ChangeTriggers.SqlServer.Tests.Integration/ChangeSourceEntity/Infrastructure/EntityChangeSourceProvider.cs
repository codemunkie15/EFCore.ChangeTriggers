using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Infrastructure;

internal class EntityChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }

    public ChangeSource CurrentChangeSourceAsync { get; set; }
}
