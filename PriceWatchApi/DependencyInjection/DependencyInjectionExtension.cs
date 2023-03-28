using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using PriceWatchApi.Settings;
using System.Data;
using System.Data.SqlClient;

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

            services.AddTransient<IDbConnection>(provider =>
            {
                String? connectionString = configuration.GetConnectionString("MSSQL");

                return new SqlConnection(connectionString);
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
            services.AddScoped<IProductWebsiteRepository, ProductWebsiteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWebsiteRepository, WebsiteRepository>();

            return services;
        }
    }
}