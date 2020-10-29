using System.Collections.Generic;
using System.Linq;

namespace Authentication.Models
{
    public static class FixedUsers
    {
        private static List<ApplicationUser> FixedList => new List<ApplicationUser>
        {
           new  ApplicationUser { Id="1450c709-f24a-42fe-8c3a-299a098a0f90",
                                  Credentials= new Credentials{ Username="Hoan", Password="123qwe" }, 
                                  Roles= new string[]{ UserRoles.User } },

           new  ApplicationUser { Id="c84c4f3f-0d52-426f-bb23-8429fd6739d5",
                                  Credentials= new Credentials{ Username="Admin", Password="123qwe" },
                                  Roles= new string[]{ UserRoles.Admin } }
        };

        public static (bool Success, UserInfo User) CheckLoginCredentials(Credentials input)
        {
            var user = FixedList.FirstOrDefault(x => x.Credentials.Username == input.Username
                                                && x.Credentials.Password == input.Password);
            if (user == null)
                return (false, null);

            return (true, user.ToUserInfo());
        }

        public static ApplicationUser FindUserByUsername(string username)
        {
            return FixedList.FirstOrDefault(x => x.Credentials.Username == username);
        }
    }
}