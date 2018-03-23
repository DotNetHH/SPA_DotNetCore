using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Data;

namespace WebAPI.Configuration
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity(
            this IServiceCollection services,
            IHostingEnvironment env,
            IConfiguration config)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            if (env.IsDevelopment())
            {
                return services;
            }

            services.ConfigureApplicationCookie(options =>
            {
                // Unterscheidung nach env hier eigentlich nicht nötig - belassen, um Optionen aufzuzeigen
                options.LoginPath = env.IsDevelopment()
                    ? "/Account/DevSignIn"
                    : "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Events.OnRedirectToLogin = ctx =>
                {
                    if (ctx.Request.Path.StartsWithSegments("/api")
                        && ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                    return Task.CompletedTask;
                };
            });

            // Beispiel für 3rd Party Auth - für Config-Daten siehe https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=visual-studio
            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = config["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"];
                });

            return services.AddAuthorization();
        }
    }
}
