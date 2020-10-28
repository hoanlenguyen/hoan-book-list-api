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
        private readonly BookService _bookService;

        public HoanBookListAPI(BookService bookService)
        {
            _bookService = bookService;
        }

        [FunctionName("ConnStr")]
        public IActionResult ConnectionString(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get"/*, "post"*/, Route = null)] HttpRequest req,
            ILogger log)
        {
            //log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult(_bookService.GetConnectionString());
        }

        [FunctionName(nameof(GetBooks))]
        public IActionResult GetBooks(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get"/*, "post"*/, Route = null)] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult(_bookService.Get());
        }

        [FunctionName(nameof(GetBookById))]
        public IActionResult GetBookById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get"/*, "post"*/, Route = null)] HttpRequest req,
            ILogger log)
        {
            string id = req.Query["id"];

            if (string.IsNullOrEmpty(id))
                return null;

            return new OkObjectResult(_bookService.Get(id));
        }

        [FunctionName(nameof(PageIndexing))]
        public async Task<IActionResult> PageIndexing(
            [HttpTrigger(AuthorizationLevel.Anonymous, /*"get",*/ "post", Route = null)] HttpRequest req /*, ILogger log*/)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var request = JObject.Parse(requestBody)["request"].ToObject<PagingRequest>();

            var filter = JObject.Parse(requestBody)["filter"].ToObject<BookFilter>();

            return new OkObjectResult(_bookService.PageIndexingItems(request, filter));
        }
    }
}