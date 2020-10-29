using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Services
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddScoped<JwtAuthenticationService>();

            return services;
        }
    }
}