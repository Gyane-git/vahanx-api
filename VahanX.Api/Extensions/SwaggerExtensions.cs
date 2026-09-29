using Microsoft.OpenApi.Models;
using VahanX.Api.Configuration;

namespace VahanX.Api.Extensions;

/// <summary>
/// Extension methods for Swagger/OpenAPI configuration.
/// </summary>
public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetSection(SwaggerSettings.SectionName).Get<SwaggerSettings>() ?? new SwaggerSettings();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = swaggerSettings.Title,
                Version = swaggerSettings.Version,
                Description = swaggerSettings.Description,
                Contact = new OpenApiContact
                {
                    Name = "VahanX",
                    Email = "support@vahanx.com"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app, IConfiguration configuration)
    {
        var swaggerSettings = configuration.GetSection(SwaggerSettings.SectionName).Get<SwaggerSettings>() ?? new SwaggerSettings();

        if (swaggerSettings.Enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "VahanX API v1");
                options.RoutePrefix = "swagger";
            });
        }

        return app;
    }
}
