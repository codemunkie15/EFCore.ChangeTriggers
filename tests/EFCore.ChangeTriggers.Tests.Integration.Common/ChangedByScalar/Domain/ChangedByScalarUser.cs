using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangedByScalar.Domain;

public class ChangedByScalarUser : BaseUser, ITracked<ChangedByScalarUserChange>
{
    public ICollection<ChangedByScalarUserChange> Changes { get; set; }

    public static ChangedByScalarUser SystemUser { get; } = new ChangedByScalarUser { Id = 1 };
}
