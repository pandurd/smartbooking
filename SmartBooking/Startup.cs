using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using SmartBooking.Clients;
using SmartBooking.Models;
using SmartBooking.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace SmartBooking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            //jitter retry would be also nice
            //retry 2 times wih exponential time
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                                                                            retryAttempt)));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var accessToken = Environment.GetEnvironmentVariable("ACESSTOKEN");
           
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            //Http client with service
            services.AddHttpClient("serviceClient", client => 
                   client.DefaultRequestHeaders.Add("x-access-token", accessToken))
                .SetHandlerLifetime(TimeSpan.FromMinutes(3))
                .AddPolicyHandler(GetRetryPolicy());

            services.Configure<List<ServicesConfigURL>>(Configuration.GetSection("MovieServices"));
            services.Configure<List<string>>(Configuration.GetSection("MovieProviders"));

            services.AddScoped<IMovieClientFactory, MovieClientFactory>();
            services.AddScoped<IMovieService, MovieService>();
            
            //should be in redis as it will better for multiple instances,
            //for now configured to be in memory
            //should reduce api call costs, if api hits increase costs
            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartBooking", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartBooking v1"));

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
