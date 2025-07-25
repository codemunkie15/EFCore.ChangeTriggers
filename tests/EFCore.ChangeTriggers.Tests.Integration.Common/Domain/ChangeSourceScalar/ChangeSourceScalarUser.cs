﻿using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Domain.ChangeSourceScalar;

public class ChangeSourceScalarUser : UserBase, ITracked<ChangeSourceScalarUserChange>
{
    public ICollection<ChangeSourceScalarUserChange> Changes { get; set; }

    public static ChangeSourceScalarUser SystemUser { get; } = new ChangeSourceScalarUser { Id = 1 };
}
