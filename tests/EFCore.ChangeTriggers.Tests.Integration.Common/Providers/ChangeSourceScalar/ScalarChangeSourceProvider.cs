using EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Providers.ChangeSourceScalar;

public class ScalarChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }

    public ChangeSource CurrentChangeSourceAsync { get; set; }

    public ChangeSource MigrationChangeSource { get; set; }

    public ChangeSource MigrationChangeSourceAsync { get; set; }
}
