using AspNetCore.Identity.Mongo;
using Authentication.Models.Identity;
using Authentication.Services;
using HoanBookListAPI.Middleware;
using HoanBookListData.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDbConnection.Settings;
using System.Threading.Tasks;

namespace HoanBookListAPI
{
    public class Program
    {
        public static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("host.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables().Build();

                    //var configRoot = config.AddEnvironmentVariables().Build();
                    //config.AddAzureAppConfiguration(configRoot["AppConfigurationConnectionString"]);
                })
                .ConfigureFunctionsWorkerDefaults(workerApplication =>
                {
                    // Register custom middleware with the worker
                    //workerApplication.UseMiddleware<MyCustomMiddleware>();
                })
                .ConfigureServices(service =>
                {
                    service.AddOptions<MongoDbConnectionSettings>()
                        .Configure<IConfiguration>((settings, config) =>
                            config.GetSection(DbConnectionConfigs.MongoDBConnectionSetting).Bind(settings));

                    service.AddSingleton<IMongoDbConnectionSettings>(sp =>
                            sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

                    //service.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(identityOptions =>
                    //{
                    //    identityOptions.Password.RequiredLength = 6;
                    //    identityOptions.Password.RequireLowercase = false;
                    //    identityOptions.Password.RequireUppercase = false;
                    //    identityOptions.Password.RequireNonAlphanumeric = false;
                    //    identityOptions.Password.RequireDigit = false;
                    //}, mongoIdentityOptions =>
                    //    {
                    //        var settings = new MongoDbConnectionSettings();
                    //        mongoIdentityOptions.ConnectionString = settings.ConnectionString;
                    //    });

                    service.AddSingleton<MongoDbContext>();

                    service.AddAuthenticationServices();

                    service.AddSingleton<IBookService, BookService>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}