using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HoanBookListAPI
{
    public class AuthenticationAPI
    {
        private readonly JwtAuthenticationService _jwtAuth;
        public AuthenticationAPI(JwtAuthenticationService jwtAuth)
        {
            _jwtAuth = jwtAuth;
        }

        [FunctionName(nameof(Authenticate))]
        public async Task<IActionResult> Authenticate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")]
            Credentials credentials,
            ILogger log)
        {
            var (success, token, user) = _jwtAuth.Login(credentials);

            if (!success)
                return new UnauthorizedResult();

            return new OkObjectResult(new { token, user });
        }

        [FunctionName(nameof(VerifyUser))]
        public async Task<IActionResult> VerifyUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "verify")]
            HttpRequest req,
            ILogger log)
        {
            var (verify, user) = _jwtAuth.VerifyUser(req);

            if (!verify)
                return new UnauthorizedResult();

            return new OkObjectResult(user);
        }
    }
}