using Authentication.Services;
using HoanBookListData.Models;
using HoanBookListData.Models.Paging;
using HoanBookListData.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace HoanBookListAPI
{
    public class HoanBookListAPI
    {
        private readonly JwtAuthenticationService _jwtAuth;
        private readonly BookService _bookService;

        public HoanBookListAPI(BookService bookService, JwtAuthenticationService jwtAuth)
        {
            _bookService = bookService;
            _jwtAuth = jwtAuth;
        }

        [FunctionName("ConnStr")]
        public IActionResult ConnectionString(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mongodb/connstr")] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");
            var (verify, user) = _jwtAuth.VerifyUser(req);

            if (!verify)
                return new UnauthorizedResult();

            return new OkObjectResult(_bookService.GetConnectionString());
        }

        [FunctionName(nameof(GetBooks))]
        public IActionResult GetBooks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books")] HttpRequest req,
            ILogger log)
        {
            //var (verify, user) = _jwtAuth.VerifyUser(req);

            //if (!verify)
            //    return new UnauthorizedResult();

            return new OkObjectResult(_bookService.Get());
        }

        [FunctionName(nameof(GetBookById))]
        public IActionResult GetBookById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books/{id}")] 
            HttpRequest req, string id, ILogger log)
        {
            if (string.IsNullOrEmpty(id))
                return new BadRequestObjectResult("Id is not found!");

            var (verify, user) = _jwtAuth.VerifyUser(req);

            if (!verify)
                return new UnauthorizedResult();            

            return new OkObjectResult(_bookService.Get(id));
        }

        [FunctionName(nameof(PageIndexing))]
        public async Task<IActionResult> PageIndexing(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "books/paging")] HttpRequest req)
        {
            var (verify, user) = _jwtAuth.VerifyUser(req);

            if (!verify)
                return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var request = JObject.Parse(requestBody)["request"].ToObject<PagingRequest>();

            var filter = JObject.Parse(requestBody)["filter"].ToObject<BookFilter>();

            return new OkObjectResult(_bookService.PageIndexingItems(request, filter));
        }

        [FunctionName("Test")]
        public static async Task<IActionResult> Test(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequest req
        )
        {
            var data = new { id = 1, name = "Hoan" };
            return new OkObjectResult(data);
        }
    }
}