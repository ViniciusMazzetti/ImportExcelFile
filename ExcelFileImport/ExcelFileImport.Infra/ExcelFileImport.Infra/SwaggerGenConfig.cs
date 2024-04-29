using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ExcelFileImport.Infra
{
    public static class SwaggerGenConfig
    {
        private const string SwaggerTitle = "Excel File Import";

        private const string SwaggerVersion = "v1";

        public static IServiceCollection ConfigureSwaggerGenServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new OpenApiInfo
                {
                    Title = SwaggerTitle,
                    Version = SwaggerVersion,
                    Description = $"Build: {configuration["BuildNumber"]}"
                });
            });

            return services;
        }
    }
}