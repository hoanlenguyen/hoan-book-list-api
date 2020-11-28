using AspNetCore.Identity.Mongo.Model;

namespace Authentication.Models.Indentity
{
    public class ApplicationUser : MongoUser
    {
    }

    public static class AppUserExtensions
    {
        public static UserInfo ToUserInfo(this ApplicationUser user)
        {
            return new UserInfo
            {
                Id = user.Id.ToString(),
                Username = user.UserName,
                Email = user.Email,
                Roles= user.Roles
            };
        }
    }
}