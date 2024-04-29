using ExcelFileImport.Infra;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ExcelFileImport.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup(
        IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public void ConfigureServices(
            IServiceCollection services)
        {
            SetConfigureServices(services);
        }

        protected virtual void SetConfigureServices(
            IServiceCollection services)
        {

            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddEndpointsApiExplorer();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.CreateDB(_configuration);
            services.ConfigureSwaggerGenServices(_configuration);
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue; // Set maximum request body size
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            SetAppConfigure(app, env);
        }

        protected virtual void SetAppConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            try
            {
                Console.WriteLine($"Server starting...");

                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

                Console.WriteLine($"Server started...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server terminated unexpectedly.\n\n Error Message: {ex}");
            }
        }
    }
}
