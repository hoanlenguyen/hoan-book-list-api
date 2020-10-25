using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HoanBookListData.MongoDb
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDbContext(this IServiceCollection services)
        {
            //    services.Configure<MongoDbConnectionSettings>(config =>
            //               configuration.GetSection(DbConnectionConfigs.MongoDBConnectionSetting));

            //    services.TryAddSingleton<IMongoDbConnectionSettings>(sp =>
            //                sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

            services.TryAddSingleton<MongoDbContext>();

            return services;
        }

        //public static IServiceCollection AddMongoDbContext(this IServiceCollection services, IConfigurationRoot configuration)
        //{
        //    services.Configure<MongoDbConnectionSettings>(config =>
        //               configuration.GetSection(DbConnectionConfigs.MongoDBConnectionSetting));

        //    services.TryAddSingleton<IMongoDbConnectionSettings>(sp =>
        //                sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

        //    services.TryAddSingleton<MongoDbContext>();

        //    return services;
        //}
    }
}