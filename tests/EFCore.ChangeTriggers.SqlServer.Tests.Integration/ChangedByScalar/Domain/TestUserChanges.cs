using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;

public class ScalarUserChange : IChange<ScalarUser>, IHasChangedBy<string>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ScalarUser TrackedEntity { get; set; }

    public string ChangedBy { get; set; }
}
