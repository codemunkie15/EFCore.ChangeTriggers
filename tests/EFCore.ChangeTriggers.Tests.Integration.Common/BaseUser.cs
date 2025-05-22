namespace EFCore.ChangeTriggers.Tests.Integration.Common
{
    public class BaseUser
    {
        public int Id { get; set; }

        public string Username { get; set; } = Guid.NewGuid().ToString();

        public string DateOfBirth { get; set; }

        public bool IsAdmin { get; set; }

        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
