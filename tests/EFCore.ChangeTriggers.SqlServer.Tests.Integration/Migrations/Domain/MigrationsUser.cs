using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;

public class MigrationsUser : ITracked<MigrationsUserChange>
{
    public int Id { get; set; }

    public string Username { get; set; }

    public ICollection<MigrationsUserChange> Changes { get; set; }
}
