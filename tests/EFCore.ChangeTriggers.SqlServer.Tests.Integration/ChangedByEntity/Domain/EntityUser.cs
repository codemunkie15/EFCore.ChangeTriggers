using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByEntity.Domain;

public class EntityUser : ITracked<EntityUserChange>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public ICollection<EntityUserChange> Changes { get; set; }
}
