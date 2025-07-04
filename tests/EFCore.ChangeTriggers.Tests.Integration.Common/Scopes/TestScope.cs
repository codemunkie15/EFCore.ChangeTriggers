using EFCore.ChangeTriggers.Tests.Integration.Common.Domain;
using EFCore.ChangeTriggers.Tests.Integration.Common.Persistence;

namespace EFCore.ChangeTriggers.Tests.Integration.Common.Scopes
{
    public class TestScope : TestScope<TestDbContext, User, UserChange>
    {
        public TestScope(IServiceProvider services) : base(services)
        {
        }
    }
}