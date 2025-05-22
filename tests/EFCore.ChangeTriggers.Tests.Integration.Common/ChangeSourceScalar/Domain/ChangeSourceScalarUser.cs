using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.ChangeSourceScalar.Domain;

public class ChangeSourceScalarUser : BaseUser, ITracked<ChangeSourceScalarUserChange>
{
    public ICollection<ChangeSourceScalarUserChange> Changes { get; set; }

    public static ChangeSourceScalarUser SystemUser { get; } = new ChangeSourceScalarUser { Id = 1 };
}
