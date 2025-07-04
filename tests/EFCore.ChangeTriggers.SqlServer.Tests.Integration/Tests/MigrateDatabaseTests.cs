using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests
{
    public class MigrateDatabaseTests :
        MigrateDatabaseTestBase<User, UserChange, TestDbContext>,
        IClassFixture<MigrateDatabaseFixture>
    {
        public MigrateDatabaseTests(MigrateDatabaseFixture fixture) : base(fixture.Services)
        {
        }
    }
}