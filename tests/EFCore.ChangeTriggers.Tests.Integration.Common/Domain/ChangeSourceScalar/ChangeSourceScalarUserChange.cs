using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;

public class ChangeSourceScalarUserChange : BaseUser, IChange<ChangeSourceScalarUser>, IHasChangeSource<ChangeSource>
{
    public int ChangeId { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangeSourceScalarUser TrackedEntity { get; set; }

    public ChangeSource ChangeSource { get; set; }
}
