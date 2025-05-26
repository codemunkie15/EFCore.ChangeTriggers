using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;

public class ChangeSourceEntityUserChange : BaseUser, IChange<ChangeSourceEntityUser>, IHasChangeSource<ChangeSource>
{
    public int ChangeId { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangeSourceEntityUser TrackedEntity { get; set; }

    public ChangeSource ChangeSource { get; set; }
}
