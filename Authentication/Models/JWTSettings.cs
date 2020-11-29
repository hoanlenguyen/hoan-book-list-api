namespace Authentication.Models
{
    internal static class JWTSettings
    {
        public const string SecretKey = "$jwts-are-awesome$";

        public const string Issuer = "https://localhost";

        public const string Audience = "https://localhost";
    }
}