using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Services
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<JwtAuthenticationService>();

            services.AddScoped<IdentityService>();

            return services;
        }
    }
}