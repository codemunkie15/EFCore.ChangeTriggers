using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;

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
