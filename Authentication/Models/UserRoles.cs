using System.Collections.Generic;

namespace Authentication.Models
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Guess = "Guess";

        public static List<string> GetRoles() => new List<string> { Admin, User, Guess };
    }
}