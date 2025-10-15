
using Microsoft.OpenApi.Models;

namespace BE_eMotoCare.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerDependencies(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Modern Estate API",
                        Description = "API for Modern Estate",
                        TermsOfService = new Uri("https://example.com/terms"),
                        Contact = new OpenApiContact
                        {
                            Name = "Mordern Estate",
                            Email = "mordernestate@gmail.com",
                        },
                        License = new OpenApiLicense
                        {
                            Name = "Use under LICX",
                            Url = new Uri("https://example.com/license"),
                        },
                    }
                );
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert JWT with Bearer into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                    }
                );
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer",
                                },
                            },
                            new string[] { }
                        },
                    }
                );
            });
            return services;
        }

    }
}