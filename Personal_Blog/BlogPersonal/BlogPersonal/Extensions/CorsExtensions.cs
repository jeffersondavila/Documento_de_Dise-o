using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogPersonal.Extensions
{
	public static class CorsExtensions
	{
		public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
		{
			// Obtener la URL del front-end desde el appsettings
			var frontEndUrl = configuration["Cors:FrontEndUrl"];

			services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigins", builder =>
				{
					builder.WithOrigins(frontEndUrl!)
						   .AllowAnyHeader()
						   .AllowAnyMethod();
				});
			});
			return services;
		}

		public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
		{
			app.UseCors("AllowSpecificOrigins");
			return app;
		}
	}
}
