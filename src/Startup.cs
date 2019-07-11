// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------

namespace Provider
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.AspNetCore.Routing.Constraints;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The start up class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"> Collection of service descriptor.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder for configuring routes etc...</param>
        /// <param name="env">web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseMvc(this.ConfigureRoutes);
        }

        /// <summary>
        /// Configures the routes.
        /// </summary>
        /// <param name="routeBuilder">Route builder.</param>
        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            this.ConfigureRoutesForProviderRegistrationController(routeBuilder);
        }

        /// <summary>
        /// Configures the routes for provider registration controller.
        /// </summary>
        /// <param name="routeBuilder">Route builder.</param>
        private void ConfigureRoutesForProviderRegistrationController(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "OnResourceCreationValidate",
                template: "subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/{providerNamespace}/{resourceType}/{resourceTypeName}/resourceCreationValidate",
                defaults: new { controller = "Provider", action = "OnResourceCreationValidate" },
                constraints: new { httpMethod = new HttpMethodRouteConstraint(new[] { "POST" }) });

            routeBuilder.MapRoute(
                name: "OnResourceCreationCompleted",
                template: "subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/{providerNamespace}/{resourceType}/{resourceTypeName}/resourceCreationCompleted",
                defaults: new { controller = "Provider", action = "OnResourceCreationCompleted" },
                constraints: new { httpMethod = new HttpMethodRouteConstraint(new[] { "POST" }) });

            routeBuilder.MapRoute(
                name: "OnResourceDeletionValidate",
                template: "subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/{providerNamespace}/{resourceType}/{resourceTypeName}/resourceDeletionValidate",
                defaults: new { controller = "Provider", action = "OnResourceDeletionValidate" },
                constraints: new { httpMethod = new HttpMethodRouteConstraint(new[] { "POST" }) });

            routeBuilder.MapRoute(
                name: "OnResourceCreationBegin",
                template: "subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/{providerNamespace}/{resourceType}/{resourceTypeName}",
                defaults: new { controller = "Provider", action = "OnResourceCreationBegin" },
                constraints: new { httpMethod = new HttpMethodRouteConstraint(new[] { "PUT" }) });
        }
    }
}
