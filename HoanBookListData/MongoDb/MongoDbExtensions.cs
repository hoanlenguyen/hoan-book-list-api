using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace HoanBookListData.MongoDb
{
    public static class MongoDbExtensions
    {
        public static IServiceCollection AddMongoDbContext(this IServiceCollection services)
        {
            services.AddOptions<MongoDbConnectionSettings>()
                    .Configure<IConfiguration>((settings, configuration) =>
                    {
                        configuration.GetSection(DbConnectionConfigs.MongoDBConnectionSetting);
                    });

            services.TryAddSingleton<IMongoDbConnectionSettings>(sp =>
                            sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

            services.TryAddSingleton<MongoDbContext>();

            return services;
        }

        public static IServiceCollection AddMongoDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConnectionSettings>(config =>
                       configuration.GetSection(DbConnectionConfigs.MongoDBConnectionSetting));

            services.TryAddSingleton<IMongoDbConnectionSettings>(sp =>
                        sp.GetRequiredService<IOptions<MongoDbConnectionSettings>>().Value);

            services.TryAddSingleton<MongoDbContext>();

            return services;
        }
    }
}