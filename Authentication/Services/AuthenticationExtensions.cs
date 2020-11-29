using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Services
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddSingleton<JwtAuthenticationService>();

            services.AddScoped<IdentityService>();

            return services;
        }
    }
}