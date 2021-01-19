using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer_Example
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddInMemoryClients(new List<Client>
                {
                    new Client
                    {
                        ClientId = "platformApiClient",
                        ClientName = "Example client application",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                        AllowedScopes = new List<string> {"quasar.read:*/*"}
                    }
                })
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource
                    {
                        Name = "quasar",
                        DisplayName = "Quasar messaging",
                        Scopes = new List<string> {"quasar.read:*/*", "api1.write"},
                    }
                })
                .AddInMemoryApiScopes(new List<ApiScope>()
                {
                    new ApiScope("quasar.read:*/*", "Read access to RabbitMQ"),
                })
                .AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
        }
    }
}