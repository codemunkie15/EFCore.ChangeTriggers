using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Infrastructure;

internal class ScalarChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }

    public ChangeSource CurrentChangeSourceAsync { get; set; }
}
