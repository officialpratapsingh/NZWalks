using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace NZWalks.API.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public String Username{ get; set; }

        public String EmailAddress { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Password { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }
        // navigation properties 
        public List<User_Roles> UserRoles { get; set; }

    }
}
