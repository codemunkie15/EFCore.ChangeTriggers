using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;

public class ChangedByScalarUserChange : IChange<ChangedByScalarUser>, IHasChangedBy<string>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangedByScalarUser TrackedEntity { get; set; }

    public string ChangedBy { get; set; }
}
