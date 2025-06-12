using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests;

public class MutateEntityTests :
    MutateEntityTestBase<User, UserChange, TestDbContext>,
    IClassFixture<MutateEntityFixture>
{
    public MutateEntityTests(MutateEntityFixture fixture) : base(fixture.Services)
    {
    }
}