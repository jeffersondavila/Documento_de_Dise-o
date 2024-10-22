using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogPersonal.Extensions
{
	public static class CorsExtensions
	{
		public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
		{
			// Obtener URL del front-end desde el appsettings
			var frontEndUrl = configuration["Cors:FrontEndUrl"];

			// Configurar políticas CORS
			services.AddCors(options =>
			{
				// Se define una política llamada "AllowSpecificOrigins" que define los orígenes permitidos
				options.AddPolicy("AllowSpecificOrigins", builder =>
				{
					builder.WithOrigins(frontEndUrl!) // Permitir solicitudes solo desde la URL del frontend
						   .AllowAnyHeader() // Permitir cualquier encabezado en la solicitud
						   .AllowAnyMethod(); // Permitir cualquier método HTTP (GET, POST, PUT, DELETE, etc)
				});
			});

			// Retorna IServiceCollection para permitir la cadena de configuraciones
			return services;
		}

		public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
		{
			// Aplicar la política CORS llamada "AllowSpecificOrigins"
			app.UseCors("AllowSpecificOrigins");
			return app;
		}
	}
}
