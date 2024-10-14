using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangeSourceEntity.Domain;

public class ChangeSourceEntityUser : ITracked<ChangeSourceEntityUserChange>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public ICollection<ChangeSourceEntityUserChange> Changes { get; set; }
}
