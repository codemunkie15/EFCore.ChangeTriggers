using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedBy.Persistence;

public class TestUserChange : IChange<TestUser>, IHasChangedBy<TestUser>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public TestUser TrackedEntity { get; set; }

    public TestUser ChangedBy { get; set; }
}
