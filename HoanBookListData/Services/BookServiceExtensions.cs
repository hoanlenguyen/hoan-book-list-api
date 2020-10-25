using HoanBookListData.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HoanBookListData.Services
{
    public static class BookServiceExtensions
    {
        public static IServiceCollection AddBookListServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddBookListServices();

            return services;
        }

        public static IServiceCollection AddBookListServices(this IServiceCollection services)
        {
            services.TryAddScoped<BookService>();

            return services;
        }
    }
}