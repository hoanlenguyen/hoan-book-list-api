//using Authentication.Models.Identity;
//using Authentication.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Extensions.Http;
//using System.Threading.Tasks;

//namespace HoanBookListAPI
//{
//    public class AuthenticationAPI
//    {
//        private readonly IdentityService _identify;
//        private readonly JwtAuthenticationService _jwtService;

//        public AuthenticationAPI(IdentityService identify, JwtAuthenticationService jwtService)
//        {
//            _identify = identify;
//            _jwtService = jwtService;
//        }

//        [FunctionName(nameof(Register))]
//        public async Task<IActionResult> Register(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")]
//            RegisterModel model)
//        {
//            var (success, token, user) = await _identify.RegisterAsync(model);
//            if (!success)
//                return new UnauthorizedResult();

//            return new OkObjectResult(new { user, token });
//        }

//        [FunctionName(nameof(Authenticate))]
//        public async Task<IActionResult> Authenticate(
//            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")]
//            LoginModel model)
//        {
//            var (success, token, user) = await _identify.LoginAsync(model);

//            if (!success)
//                return new UnauthorizedResult();

//            return new OkObjectResult(new { token, user });
//        }

//    }
//}