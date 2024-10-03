using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

public class EntityUserChange : IChange<EntityUser>, IHasChangedBy<EntityUser>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public EntityUser TrackedEntity { get; set; }

    public EntityUser ChangedBy { get; set; }
}
