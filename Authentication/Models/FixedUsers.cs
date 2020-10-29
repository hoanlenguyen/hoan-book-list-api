using System.Collections.Generic;
using System.Linq;

namespace Authentication.Models
{
    public static class FixedUsers
    {
        private static List<ApplicationUser> FixedList => new List<ApplicationUser>
        {
           new  ApplicationUser { Credentials= new Credentials{ Username="Hoan", Password="123qwe" }, Roles= new string[]{ UserRoles.User } },
           new  ApplicationUser { Credentials= new Credentials{ Username="Admin", Password="123qwe" }, Roles= new string[]{ UserRoles.Admin } }
        };

        public static (bool Success, ApplicationUser User) CheckLoginCredentials(Credentials input)
        {
            var user = FixedList.FirstOrDefault(x => x.Credentials.Username == input.Username
                                                && x.Credentials.Password == input.Password);
            if (user == null)
                return (false, null);

            return (true, user);
        }

        public static ApplicationUser FindUserByUsername(string username)
        {
            return FixedList.FirstOrDefault(x => x.Credentials.Username == username);
        }
    }
}