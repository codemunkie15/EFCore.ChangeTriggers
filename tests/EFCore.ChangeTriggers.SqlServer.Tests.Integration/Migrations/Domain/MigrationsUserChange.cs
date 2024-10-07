using EFCore.ChangeTriggers.Abstractions;
using EFCore.ChangeTriggers.Models;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Migrations.Domain;

public class MigrationsUserChange : IChange<MigrationsUser>, IHasChangedBy<MigrationsUser>, IHasChangeSource<ChangeSource>
{
    public int ChangeId { get; set; }

    public int Id { get; set; }

    public string Username { get; set; }

    public OperationType OperationType { get; set; }

    public DateTimeOffset ChangedAt { get; set; }

    public MigrationsUser TrackedEntity { get; set; }

    public MigrationsUser ChangedBy { get; set; }

    public ChangeSource ChangeSource { get; set; }
}
