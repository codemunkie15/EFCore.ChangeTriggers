using EFCore.ChangeTriggers.Abstractions;

namespace EFCore.ChangeTriggers.SqlServer.Tests.Integration
{
    internal class TestUser : ITracked<TestUserChange>
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public ICollection<TestUserChange> Changes { get; set; }
    }
}
