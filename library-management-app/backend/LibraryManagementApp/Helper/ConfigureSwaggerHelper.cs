using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LibraryManagementApp.Helper
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "Library Management API",
                    Version = description.ApiVersion.ToString(),
                    Description = @$"This project is a comprehensive library management application developed using .NET 9 and C#.
                                     It features a robust architecture following MVC 4.0 standards and SOLID principles, designed to efficiently manage book loans, including user authentication with a login screen and advanced book search capabilities. 
                                     The entire solution is containerized using Docker and orchestrated with Kubernetes, ensuring scalability, portability, and ease of deployment across various environments.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ademir Purin",
                        Email = "jnrpurin@gmail.com",
                        Url = new Uri("https://github.com/jnrpurin")
                    }
                });
            }
        }
    }
}