

//using HoanBookListAPI.Middleware;
//using Microsoft.Extensions.Hosting;

//namespace HoanBookListAPI
//{
//   public class Program
//    {
//        public static void Main()
//        {
//            //<docsnippet_middleware_register>
//            var host = new HostBuilder()
//            .ConfigureFunctionsWorkerDefaults(
//                builder =>
//                {
//                    builder.UseMiddleware<MyCustomMiddleware>();
//                }
//            )
//            .Build();
//            //</docsnippet_middleware_register>
//            host.Run();
//        }
//    }
//}
