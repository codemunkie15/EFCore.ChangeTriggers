using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.ChangedByScalar.Domain;

public class ChangedByScalarUser : ITracked<ChangedByScalarUserChange>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public ICollection<ChangedByScalarUserChange> Changes { get; set; }

    public ChangedByScalarUser()
    {
        
    }

    public ChangedByScalarUser(int id, string username)
    {
        Id = id;
        Username = username;
    }
}
