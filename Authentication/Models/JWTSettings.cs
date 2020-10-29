namespace Authentication.Models
{
    internal static class JWTSettings /*: IJWTSettings*/
    {
        public const string SecretKey = "$jwts-are-awesome$";

        public const string Issuer = "https://localhost";

        public const string Audience = "https://localhost";
    }

    //internal interface IJWTSettings
    //{
    //    public string SecretKey { get; }
    //    public string Issuer { get; }
    //    public string Audience { get; }
    //}
}