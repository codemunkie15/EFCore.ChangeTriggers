using EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests.Fixtures;
using EFCore.ChangeTriggers.Tests.Integration.Common.Tests;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration.Tests
{
    public class ExcludedChangePropertiesTests : ExcludedChangePropertiesTestBase, IClassFixture<ExcludedChangePropertiesFixture>
    {
        public ExcludedChangePropertiesTests(ExcludedChangePropertiesFixture fixture) : base(fixture.Services)
        {
        }
    }
}
