using Authentication.Models;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Authentication.Services
{
    public class JwtAuthenticationService
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;

        public JwtAuthenticationService()
        {
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
        }

        public (bool Success, string Token) Login(Credentials credentials)
        {
            var (success, user) = FixedUsers.CheckLoginCredentials(credentials);
            if (!success)
                return (false, "");

            var claims = new Dictionary<string, object>
            {
                { "username", user.Credentials.Username },
                { "role", user.Roles}
            };

            var token = _jwtEncoder.Encode(claims, JWTSettings.SecretKey);

            return (true, token);
        }

        public (bool IsValid, ApplicationUser user) VerifyUser(HttpRequest request)
        {

            string bearerToken = request.Headers["Authorization"];
            if (string.IsNullOrEmpty(bearerToken))
                return (false, null);

            var token = bearerToken.StartsWith("Bearer") ? bearerToken.Substring(7) : bearerToken;

            if (string.IsNullOrEmpty(token))
                return (false, null);

            var claims = new JwtBuilder()
               .WithAlgorithm(new HMACSHA256Algorithm())
               .WithSecret(JWTSettings.SecretKey)
               .MustVerifySignature()
               .Decode<IDictionary<string, object>>(token);

            if (!claims.ContainsKey("username"))
                return (false, null);

            var user = FixedUsers.FindUserByUsername(Convert.ToString(claims["username"]));
            if (user == null)
                return (false, null);

            return (true, user);
        }
    }
}