namespace Authentication.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }

        public Credentials Credentials { set; get; }

        public string[] Roles { set; get; } = new string[] { };
    }

    public class UserInfo
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string[] Roles { set; get; }

        public bool IsGuess => FixedUserRoles.CheckGuessRole(Roles);
    }

    public static class ApplicationUserExtensions
    {
        public static UserInfo ToUserInfo(this ApplicationUser user)
        {
            return new UserInfo
            {
                Id = user.Id,
                Username = user.Credentials.Username,
                Roles = user.Roles
            };
        }
    }
}