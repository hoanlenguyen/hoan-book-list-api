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
        public JwtAuthenticationService()
        {
        }

        public (bool Success, string Token, UserInfo User) Login(Credentials credentials)
        {
            var (success, user) = FixedUsers.CheckLoginCredentials(credentials);
            if (!success)
                return (false, "", null);

            var claims = new Dictionary<string, object>
            {
                { "username", user.Username },
                { "expires", DateTime.UtcNow.AddMinutes(120) },
                { "role", user.Roles}
            };

            var jwtEncoder = new JwtEncoder(new HMACSHA256Algorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder());
            var token = jwtEncoder.Encode(claims, JWTSettings.SecretKey);

            return (true, token, user);
        }

        public (bool IsValid, UserInfo user) VerifyUser(HttpRequest request)
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

            if (!claims.ContainsKey("username")|| !claims.ContainsKey("expires"))
                return (false, null);

            var expires = Convert.ToDateTime(claims["expires"]);
            if (expires < DateTime.UtcNow)
                return (false, null);

            var user = FixedUsers.FindUserByUsername(Convert.ToString(claims["username"]));
            if (user == null)
                return (false, null);

            return (true, user.ToUserInfo());
        }
    }
}