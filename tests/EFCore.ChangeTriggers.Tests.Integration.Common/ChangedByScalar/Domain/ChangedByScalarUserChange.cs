using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Domain;

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
