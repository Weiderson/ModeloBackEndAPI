using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ModeloBackEndAPI.Configure.Swagger
{
    /// <summary>
    /// Configura a geração dos documentos Swagger por versão da API.
    /// </summary>
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        /// <summary>
        /// Inicializa uma nova instância de <c>Configure Swagger Gen Options</c>.
        /// </summary>
        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
            => _apiVersionDescriptionProvider = apiVersionDescriptionProvider;

        /// <summary>
        /// Configura as opções de geração da documentação OpenAPI.
        /// </summary>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }

        private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Transação Bancária - .NET (Weiderson Mendes Queiroz)",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " (deprecated)";
            }

            return info;
        }
    }
}