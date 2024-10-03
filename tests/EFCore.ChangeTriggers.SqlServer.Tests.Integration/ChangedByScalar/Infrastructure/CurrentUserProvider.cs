namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Infrastructure;

internal class CurrentUserProvider
{
    public Guid InstanceId = Guid.NewGuid();

    public string CurrentUser { get; set; }
}
