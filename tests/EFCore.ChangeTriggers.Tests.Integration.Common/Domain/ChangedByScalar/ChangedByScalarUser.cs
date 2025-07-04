using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangedByScalar;

public class ChangedByScalarUser : UserBase, ITracked<ChangedByScalarUserChange>
{
    public ICollection<ChangedByScalarUserChange> Changes { get; set; }

    public static ChangedByScalarUser SystemUser { get; } = new ChangedByScalarUser { Id = 1 };
}
