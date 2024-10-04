using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

public class ChangedByEntityUser : ITracked<ChangedByEntityUserChange>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public ICollection<ChangedByEntityUserChange> Changes { get; set; }
}
