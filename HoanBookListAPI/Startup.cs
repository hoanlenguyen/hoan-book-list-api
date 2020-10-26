using HoanBookListData.MongoDb;
using HoanBookListData.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Reflection;

[assembly: FunctionsStartup(typeof(HoanBookListAPI.Startup))]

namespace HoanBookListAPI
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            try
            {
                var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..");
                var environmentName = builder.GetContext().EnvironmentName;
                builder.ConfigurationBuilder
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            try
            {
                var configuration = builder.GetContext().Configuration;

                builder.Services.AddMongoDbContext(configuration);

                builder.Services.AddBookListServices();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}