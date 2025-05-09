using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Domain;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceEntity.Infrastructure;

public class EntityChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }

    public ChangeSource CurrentChangeSourceAsync { get; set; }

    public ChangeSource MigrationChangeSource { get; set; }

    public ChangeSource MigrationChangeSourceAsync { get; set; }
}
