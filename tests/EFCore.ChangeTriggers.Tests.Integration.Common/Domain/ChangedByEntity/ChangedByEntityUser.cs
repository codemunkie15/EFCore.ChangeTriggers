using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByEntity;

public class ChangedByEntityUser : BaseUser, ITracked<ChangedByEntityUserChange>
{
    public ICollection<ChangedByEntityUserChange> Changes { get; set; }

    public static ChangedByEntityUser SystemUser { get; } = new ChangedByEntityUser { Id = 1 };
}