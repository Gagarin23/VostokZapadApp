using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Infrastructure.Business;
using VostokZapadApp.Infrastructure.Data;
using VostokZapadApp.Infrastructure.Data.Initialisation;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("WorkConnection");
            services.AddTransient<IDbConnection, SqlConnection>(provider => new SqlConnection(connectionString));
            services.AddSingleton<IDatabaseInitialiser, DatabaseInitialiser>(provider => new DatabaseInitialiser("VostokZapadDb"));

            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IOrdersValidateService, OrdersValidateService>();
            services.AddScoped<ICustomersValidateService, CustomersValidateService>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VostokZapadApp", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDatabaseInitialiser initialiser)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VostokZapadApp v1"));
            }

            initialiser.CreateDatabase();
            initialiser.CreateTables();
            initialiser.CreateCustomersProcedures();
            initialiser.CreateOrdersProcedures();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
