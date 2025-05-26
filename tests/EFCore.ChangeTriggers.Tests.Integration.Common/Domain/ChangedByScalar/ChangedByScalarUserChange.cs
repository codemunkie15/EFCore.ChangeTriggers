using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;

public class ChangedByScalarUserChange : BaseUser, IChange<ChangedByScalarUser>, IHasChangedBy<string>
{
    public int ChangeId { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangedByScalarUser TrackedEntity { get; set; }

    public string ChangedBy { get; set; }
}
