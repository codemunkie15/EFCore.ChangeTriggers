using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceEntity;

public class ChangeSourceEntityUser : UserBase, ITracked<ChangeSourceEntityUserChange>
{
    public ICollection<ChangeSourceEntityUserChange> Changes { get; set; }

    public static ChangeSourceEntityUser SystemUser { get; } = new ChangeSourceEntityUser { Id = 1 };
}
