using A_AspNetCore.Models.Roles;

namespace A_AspNetCore.Models.Users
{
    public class BaseUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DateOfBirth { get; set; }

        public int RoleId { get; set; }

        public Role? Role { get; set; }
    }
}