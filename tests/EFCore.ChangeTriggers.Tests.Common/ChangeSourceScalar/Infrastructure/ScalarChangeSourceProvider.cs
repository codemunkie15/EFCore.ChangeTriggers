using EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Infrastructure;

public class ScalarChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }

    public ChangeSource CurrentChangeSourceAsync { get; set; }

    public ChangeSource MigrationChangeSource { get; set; }

    public ChangeSource MigrationChangeSourceAsync { get; set; }
}
