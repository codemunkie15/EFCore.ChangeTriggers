using EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Infrastructure;

internal class ChangeSourceScalarChangeSourceProvider
{
    public ChangeSource CurrentChangeSource { get; set; }
}
