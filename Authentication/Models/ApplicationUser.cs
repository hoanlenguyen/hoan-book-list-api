namespace Authentication.Models
{
    public class ApplicationUser
    {
        public Credentials Credentials { set; get; }

        public string[] Roles { set; get; } = new string[] { };
    }
}