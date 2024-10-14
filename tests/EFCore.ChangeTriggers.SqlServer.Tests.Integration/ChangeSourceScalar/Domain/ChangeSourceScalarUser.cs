using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;

public class ChangeSourceScalarUser : ITracked<ChangeSourceScalarUserChange>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public ICollection<ChangeSourceScalarUserChange> Changes { get; set; }
}
