using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;

public class ChangeSourceEntityUserChange : IChange<ChangeSourceEntityUser>, IHasChangeSource<ChangeSource>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public ChangeSourceEntityUser TrackedEntity { get; set; }

    public ChangeSource ChangeSource { get; set; }
}
