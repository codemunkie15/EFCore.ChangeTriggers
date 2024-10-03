using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;

public class ScalarUser : ITracked<ScalarUserChange>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public ICollection<ScalarUserChange> Changes { get; set; }
}
