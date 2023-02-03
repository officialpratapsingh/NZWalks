namespace NZWalks.API.Models.Domain
{
    public class User_Roles
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User user { get; set; }

        public Guid RoleId { get; set; }
        public Role role { get; set; }
    }
}
