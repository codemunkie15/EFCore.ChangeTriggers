using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceScalar.Domain;

public class ChangeSourceScalarUserChange : IChange<ChangeSourceScalarUser>, IHasChangeSource<ChangeSource>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangeSourceScalarUser TrackedEntity { get; set; }

    public ChangeSource ChangeSource { get; set; }
}
