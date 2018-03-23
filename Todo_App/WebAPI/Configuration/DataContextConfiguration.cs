using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Data;

namespace WebAPI.Configuration
{
    public static class DataContextConfiguration
    {
        public static IServiceCollection ConfigureDataContext(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfiguration config)
        {
            if (env.IsDevelopment())
            {
                services.AddDbContext<DataContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(config.GetConnectionString("Express"));
                });
            }
            else
            {
                // Production Config. Beispielsweise Azure Web App Setting
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("todos_db")));
            }

            return services;
        }
    }
}
