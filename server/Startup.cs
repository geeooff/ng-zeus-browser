using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeusBrowser.Server.Core;
using System;

namespace ZeusBrowser.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			// app services
			services.AddZeusBrowser(Configuration.GetSection("ZeusBrowser"));

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../client/dist";
            });

			// XSRF protection
			services.AddAntiforgery(options => options.HeaderName = Core.Defaults.AntiforgeryHeaderName);
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

			// XSRF protection
			app.Use(next => context =>
			{
				if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase)
				 || string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
				{
					// We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
					var tokens = antiforgery.GetAndStoreTokens(context);

					context.Response.Cookies.Append(
						Core.Defaults.AntiforgeryCookieName,
						tokens.RequestToken,
						new CookieOptions()
						{
							HttpOnly = false
						}
					);
				}

				return next(context);
			});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "../client";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
