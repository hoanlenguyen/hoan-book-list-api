using Authentication.Services;
using HoanBookListData.MongoDb;
using HoanBookListData.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

[assembly: FunctionsStartup(typeof(HoanBookListAPI.Startup))]

namespace HoanBookListAPI
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var configuration = builder.GetContext().Configuration;

            builder.Services.AddJwtAuthentication();

            builder.Services.AddOptions<MongoDbConnectionSettings>()
                .Configure<IConfiguration>((settings, config) =>
                    config.GetSection(DbConnectionConfigs.MongoDBConnectionSetting).Bind(settings));

            builder.Services.AddSingleton<IMongoDbConnectionSettings>(sp =>
                            sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

            builder.Services.AddSingleton<MongoDbContext>();

            builder.Services.AddSingleton<BookService>();
        }
    }
}