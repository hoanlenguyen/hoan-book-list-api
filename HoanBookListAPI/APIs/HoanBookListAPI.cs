using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Authentication.Services;
using HoanBookListData.Services;

namespace HoanBookListAPI
{
    public class HoanBookListAPI
    {
        private readonly JwtAuthenticationService _jwtAuth;
        private readonly IBookService _bookService;

        public HoanBookListAPI(
            IBookService bookService,
            JwtAuthenticationService jwtAuth)
        {
            _bookService = bookService;
            _jwtAuth = jwtAuth;
        }

        [Function(nameof(GetBooks))]
        public async Task<IActionResult> GetBooks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books")] HttpRequestData req,
            ILogger log)
        {
            //var (verify, user) = _jwtAuth.VerifyUser(req);

            //if (!verify)
            //    return new UnauthorizedResult();

            return new OkObjectResult(_bookService.Get());
        }

        //[FunctionName(nameof(GetBookById))]
        //public IActionResult GetBookById(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books/{id}")]
        //    HttpRequest req, string id, ILogger log)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return new BadRequestObjectResult("Id is not found!");

        //    var (verify, user) = _jwtAuth.VerifyUser(req);

        //    if (!verify)
        //        return new UnauthorizedResult();

        //    return new OkObjectResult(_bookService.Get(id));
        //}

        //[FunctionName(nameof(PageIndexing))]
        //public async Task<IActionResult> PageIndexing(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "books/paging")] HttpRequest req)
        //{
        //    var (verify, user) = _jwtAuth.VerifyUser(req);

        //    if (!verify)
        //        return new UnauthorizedResult();

        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

        //    var request = JObject.Parse(requestBody)["request"].ToObject<PagingRequest>();

        //    var filter = JObject.Parse(requestBody)["filter"].ToObject<BookFilter>();

        //    return new OkObjectResult(_bookService.GetUiList(user.Id, request, filter));
        //}

        //[FunctionName(nameof(BookmarkBook))]
        //public IActionResult BookmarkBook(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "books/{id}/bookmark/{bookmark}")]
        //    HttpRequest req, string id, bool bookmark, ILogger log)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return new BadRequestObjectResult("Id is not found!");

        //    var (verify, user) = _jwtAuth.VerifyUser(req);

        //    if (!verify)
        //        return new UnauthorizedResult();

        //    return new OkObjectResult(_bookService.BookmarkBook(user.Id, id, bookmark));
        //}

        //[FunctionName(nameof(LikeBook))]
        //public IActionResult LikeBook(
        //   [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "books/{id}/like/{like}")]
        //    HttpRequest req, string id, bool like, ILogger log)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return new BadRequestObjectResult("Id is not found!");

        //    var (verify, user) = _jwtAuth.VerifyUser(req);

        //    if (!verify)
        //        return new UnauthorizedResult();

        //    return new OkObjectResult(_bookService.LikeBook(user.Id, id, like));
        //}
    }
}