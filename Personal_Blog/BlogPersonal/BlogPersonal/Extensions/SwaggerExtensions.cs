using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BlogPersonal.Extensions
{
	public static class SwaggerExtensions
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
		{
			// Agregar y configurar el generador de Swagger
			services.AddSwaggerGen(c =>
			{
				// Definir el documento Swagger con información básica
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Blog Personal", Version = "v1" });

				// Configuración para soportar autenticación JWT en Swagger
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization", // Encabezado
					Type = SecuritySchemeType.Http, // Esquema de seguridad
					Scheme = "Bearer", // Esquema utilizado
					BearerFormat = "JWT", // Formato del token esperado
					In = ParameterLocation.Header, // Ubicación del encabezado donde se enviará el token
					Description = "Introduce el JWT: Bearer {tu token}" // Descripcion en la interfaz
				});

				// Configuración de requerimientos de seguridad para la documentación de Swagger
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						// Especificar que el esquema de seguridad "Bearer" es requerido para acceder a la API
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme, // Referencia al esquema de seguridad
								Id = "Bearer" // Esquema utilizado
							}
						},
						Array.Empty<string>()
					}
				});
			});

			// Retorna IServiceCollection para permitir la cadena de configuraciones
			return services;
		}
	}
}
