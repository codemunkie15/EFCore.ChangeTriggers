using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;

public class ScalarChangeSourceProvider
{
    public ChangeSourceType CurrentChangeSource { get; set; }

    public ChangeSourceType CurrentChangeSourceAsync { get; set; }

    public ChangeSourceType MigrationChangeSource { get; set; }

    public ChangeSourceType MigrationChangeSourceAsync { get; set; }
}
