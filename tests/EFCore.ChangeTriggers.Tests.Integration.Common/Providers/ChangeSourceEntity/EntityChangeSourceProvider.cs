using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceEntity;

public class EntityChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }

    public ChangeSource CurrentChangeSourceAsync { get; set; }

    public ChangeSource MigrationChangeSource { get; set; }

    public ChangeSource MigrationChangeSourceAsync { get; set; }
}
