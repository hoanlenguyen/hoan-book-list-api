using Authentication.Models;
using Authentication.Models.Identity;
using JWT.Algorithms;
using JWT.Builder;
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

            if (!claims.ContainsKey("id")
                || !claims.ContainsKey("username")
                || !claims.ContainsKey("expires"))
                return (false, null);

            var expires = Convert.ToDateTime(claims["expires"]);
            if (expires < DateTime.UtcNow)
                return (false, null);

            var user = new UserInfo
            {
                Id = claims["id"].ToString(),
                Username = claims["username"].ToString(),
                //Email
                //Roles
            };

            return (true, user);
        }
    }
}