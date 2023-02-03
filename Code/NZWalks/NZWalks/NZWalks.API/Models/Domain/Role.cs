namespace NZWalks.API.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        // navigation properties 
        public List<User_Roles> UserRoles { get; set; }
    }
}
