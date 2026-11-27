using Microsoft.OpenApi.Models;

/// <summary>
/// Fornece extensões para configurar a integração do Swagger.
/// </summary>
public static class SwaggerServiceExtensions
{
    /// <summary>
    /// Registra o esquema JWT na configuração do Swagger.
    /// </summary>
    public static IServiceCollection AddSwaggerJWT(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT no campo abaixo. Exemplo: Bearer {seu token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return services;
    }
}