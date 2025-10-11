using LibraryManagementApp.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace UnitTests.HelperTests
{
    public class ConfigureSwaggerOptionsTests
    {
        [Fact]
        public void Configure_ShouldAddSwaggerDocs_ForEachApiVersion()
        {
            // Arrange
            var apiVersions = new List<ApiVersionDescription>
            {
                new(new ApiVersion(1, 0), "v1", false),
                new(new ApiVersion(2, 0), "v2", false)
            };

            var providerMock = new Mock<IApiVersionDescriptionProvider>();
            providerMock.Setup(p => p.ApiVersionDescriptions).Returns(apiVersions);

            var options = new SwaggerGenOptions();
            var configure = new ConfigureSwaggerOptions(providerMock.Object);

            // Act
            configure.Configure(options);

            // Assert
            Assert.Contains(options.SwaggerGeneratorOptions.SwaggerDocs, d => d.Key == "v1");
            Assert.Contains(options.SwaggerGeneratorOptions.SwaggerDocs, d => d.Key == "v2");
            Assert.Equal("Library Management API", options.SwaggerGeneratorOptions.SwaggerDocs["v1"].Title);
        }
    }
}
