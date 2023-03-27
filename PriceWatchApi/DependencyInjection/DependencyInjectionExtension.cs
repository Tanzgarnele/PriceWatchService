using PriceWatchApi.Settings;

namespace PriceWatchApi.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterAllDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions<ApplicationSettings>()
                    .Bind(configuration.GetSection("ApplicationSettings"));

            return services;
        }
    }
}