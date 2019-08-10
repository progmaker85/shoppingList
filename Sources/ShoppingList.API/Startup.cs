using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingList.API.Migrations;
using ShoppingList.API.Services;

namespace ShoppingList.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            LoggingService.InitLogger(logger);
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration).AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            var runner = services.AddFluentMigratorCore()
                .ConfigureRunner(
                    builder => builder
                        .AddSqlServer()
                        .WithGlobalConnectionString("Data Source=aaf3c7dzyqgsqg.cui2uwlmcxvq.eu-central-1.rds.amazonaws.com;Initial Catalog=ShoppingListDb;Integrated Security=False;User Id=admin;Password=Gfhjkm123;")
                        .ScanIn(typeof(InitDatabaseMigration).Assembly).For.Migrations())
                .BuildServiceProvider(false)
                .GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            "Check the AWS Console CloudWatch Logs console in us-east-1".ToLogInfo();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
