using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.WebEncoders;
using SimpleInjector;
using WebAPI.Configuration;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI
{
    public class Startup
    {
        private Container container = new Container();
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDataContext(this.Environment, this.Configuration);
            services.ConfigureIdentity(this.Environment, this.Configuration);
            services.Configure<WebEncoderOptions>(options => { options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All); });

            services
                .AddMvc(options =>
                {
                    if (!Environment.IsDevelopment())
                    {
                        var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .RequireClaim("Role", new[] { NutzerRollen.Aktiviert })
                            .Build();
                        options.Filters.Add(new AuthorizeFilter(policy));

                        options.Filters.Add(new RequireHttpsAttribute());
                    }
                })
                .AddJsonOptions(options => { options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; });

            services.IntegrateSimpleInjector(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, DataContext dataContext)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    ProjectPath = Path.Combine(this.Environment.WebRootPath, "../../WebClient/"),
                    ConfigFile = "webpack.config.js",
                    HotModuleReplacement = true,
                    HotModuleReplacementClientOptions = new Dictionary<string, string> { { "reload", "true" } },
                });

                // Diese Zeile muss auskommentiert werden, wenn eine Migration angelegt wird
                this.MigrateAndSeedDatabase("Express");
            }
            else
            {
                this.MigrateAndSeedDatabase("todos_migrations_db");

                // TODO geht das=?
                app.UseExceptionHandler("/Home/Error");
                app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRegistration(this.container);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Home", action = "Index" });
                });
            });
        }

        // Beispielhaft für Azure Hosting
        private void MigrateAndSeedDatabase(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(this.Configuration.GetConnectionString(connectionString));
            using (var db = new DataContext(optionsBuilder.Options))
            {
                db.Database.Migrate();

                AppDataSeed.Init(db);
            }
        }
    }
}
