using Authentication.Models;
using Authentication.Models.Identity;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public class IdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool Success, string Token, UserInfo User)> RegisterAsync(RegisterModel register)
        {
            var user = new ApplicationUser { UserName = register.Username, Email = register.Email };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
            {
                var createdUser = await _userManager.FindByNameAsync(register.Username);

                return ConvertTokenResult(createdUser);
            }

            return (false, null, null);
        }

        public async Task<(bool Success, string Token, UserInfo User)> LoginAsync(LoginModel input)
        {
            var result = await _signInManager.PasswordSignInAsync(input.Username, input.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(input.Username);

                return ConvertTokenResult(user);
            }
            return (false, null, null);
        }

        private (bool Success, string Token, UserInfo User) ConvertTokenResult(ApplicationUser user)
        {
            var claims = new Dictionary<string, object>
                            {
                                { "id", user.Id.ToString() },
                                { "username", user.UserName },
                                { "expires", DateTime.UtcNow.AddDays(1) },
                                { "role", user.Roles}
                            };

            var jwtEncoder = new JwtEncoder(new HMACSHA256Algorithm(), new JsonNetSerializer(), new JwtBase64UrlEncoder());

            var token = jwtEncoder.Encode(claims, JWTSettings.SecretKey);

            return (true, token, user.ToUserInfo());
        }
    }
}