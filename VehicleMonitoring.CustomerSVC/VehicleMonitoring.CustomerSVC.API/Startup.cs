using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using VehicleMonitoring.Common.Core.FilterAttributes;
using VehicleMonitoring.Common.Core.Repository;
using VehicleMonitoring.Common.Repository;
using VehicleMonitoring.CustomerSVC.DAL;
using VehicleMonitoring.CustomerSVC.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.CustomerSVC.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            
        }

        public IConfiguration Configuration { get; }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.Configure<GeneralAppSettings>(configurationBuilder.GetSection("GeneralConfiguration"));

            var generalSettings = services.BuildServiceProvider().GetRequiredService<IOptions<GeneralAppSettings>>().Value;

            services.AddDbContext<CustomerServiceDbContext>(options => options.UseSqlServer(generalSettings.ConnectionString));
            services.AddTransient<DbInitializer>();


            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });


            services.AddMvc(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Customers API", Version = "v1" });
            });

           
            //////////////////////

            services.AddOptions();
            services.AddTransient<DbContext, CustomerServiceDbContext>();
            services.AddTransient<IRepositoryProvider, RepositoryProvider>();
            services.AddTransient<ICustomerServiceUOW, CustomerServiceUOW>();
            services.AddTransient<IRepositoryFactory, RepositoryFactory>();
            services.AddTransient<IRepositoryProvider, RepositoryProvider>();

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
                              DbInitializer DbSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /////////////////
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customers V1");
            });
            ////////////////////////////
            app.UseCors("CorsPolicy");
            app.UseMvc();

            // Seed data through custom class
            DbSeeder.Initialize().Wait();

        }

    }
}
