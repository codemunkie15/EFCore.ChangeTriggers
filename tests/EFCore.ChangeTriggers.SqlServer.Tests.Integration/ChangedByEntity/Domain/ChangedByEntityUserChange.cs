﻿using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

public class ChangedByEntityUserChange : IChange<ChangedByEntityUser>, IHasChangedBy<ChangedByEntityUser>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangedByEntityUser TrackedEntity { get; set; }

    public ChangedByEntityUser ChangedBy { get; set; }
}
