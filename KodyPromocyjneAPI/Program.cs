using KodyPromocyjneAPI.BusinessLayer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace KodyPromocyjneAPI
{
    internal class Program
    {
        private readonly IDatabaseManagementService _databaseManagementService;

        public Program(IDatabaseManagementService databaseManagementService)
        {
            _databaseManagementService = databaseManagementService;
        }

        static void Main(string[] args)
        {
            var container = new UnityDiContainerProvider().GetContainer();
            container.Resolve<Program>().Run();

            WebHost.CreateDefaultBuilder()
                .UseUnityServiceProvider(container)
                .ConfigureServices(services =>
                {
                    services.AddMvc();
                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowOrigin",
                            builder =>
                            {
                                builder.WithOrigins(
                                    "http://localhost:10500"
                                )
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials();
                            });
                    });

                    services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kody Promocyjne", Version = "v1" });
                    });
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseCors("AllowOrigin");
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KodyPromocyjneAPI V1");
                    });
                })
                .UseUrls("http://*:10500")
                .Build()
                .Run();
        }

        void Run()
        {
            _databaseManagementService.EnsureDatabaseCreation();
        }
    }
}
