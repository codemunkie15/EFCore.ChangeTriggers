using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByEntity.Domain;

public class ChangedByEntityUserChange : BaseUser, IChange<ChangedByEntityUser>, IHasChangedBy<ChangedByEntityUser>
{
    public int ChangeId { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangedByEntityUser TrackedEntity { get; set; }

    public ChangedByEntityUser ChangedBy { get; set; }
}
