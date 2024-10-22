using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace BlogPersonal.Extensions
{
	public static class MiddlewareExtensions
	{
		public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				// Habilitar Swagger para la documentación interactiva del API
				app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					// Configurar el punto final de Swagger y el prefijo de la ruta
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Blog Personal v1");
					c.RoutePrefix = "swagger";
				});

				// Muestra la página de excepciones del desarrollador con detalles de errores
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// En un entorno de producción, se redirige a una página de error genérica
				app.UseExceptionHandler("/Home/Error");
				// Habilita HSTS (Strict-Transport-Security) para reforzar el uso de HTTPS
				app.UseHsts();
			}

			// Redirige automáticamente todas las solicitudes HTTP a HTTPS
			app.UseHttpsRedirection();
			return app;
		}
	}
}