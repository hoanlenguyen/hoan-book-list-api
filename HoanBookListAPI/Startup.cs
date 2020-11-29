using AspNetCore.Identity.Mongo;
using Authentication.Models.Indentity;
using Authentication.Services;
using HoanBookListData.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDbConnection.Settings;
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
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "host.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "local.settings.json"), optional: true, reloadOnChange: false)
                .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                //.AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;

            builder.Services.AddOptions<MongoDbConnectionSettings>()
                .Configure<IConfiguration>((settings, config) =>
                    config.GetSection(DbConnectionConfigs.MongoDBConnectionSetting).Bind(settings));

            builder.Services.AddSingleton<IMongoDbConnectionSettings>(sp =>
                            sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

            builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = false;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = config.GetSection(DbConnectionConfigs.MongoDBConnectionSetting)["ConnectionString"];
            });

            builder.Services.AddAuthenticationServices();

            builder.Services.AddSingleton<MongoDbContext>();

            builder.Services.AddSingleton<BookService>();
        }
    }
}