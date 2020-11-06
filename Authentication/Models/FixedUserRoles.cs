using System.Collections.Generic;
using System.Linq;

namespace Authentication.Models
{
    public static class FixedUserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Guess = "Guess";

        public static List<string> GetRoles() => new List<string> { Admin, User, Guess };

        public static bool CheckGuessRole(this string[] roles)
        {
            return roles.Contains(Guess);
        }
    }
}