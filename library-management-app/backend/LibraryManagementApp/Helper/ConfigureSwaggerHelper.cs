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
                    Description = "API with versioning support (v1, v2, ...)",
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