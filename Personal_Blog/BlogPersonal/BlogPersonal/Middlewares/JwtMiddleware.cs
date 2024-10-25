using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BlogPersonal.Middlewares
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _configuration;
		private readonly ILogger<JwtMiddleware> _logger;

		public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
		{
			_next = next;
			_configuration = configuration;
			_logger = logger;
		}

		// Método Invoke que se ejecuta en cada solicitud HTTP
		public async Task Invoke(HttpContext context)
		{
			// Obtiene el token JWT del encabezado "Authorization" de la solicitud, si está presente
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			// Si el token no es nulo, intenta adjuntar la identidad del usuario al contexto
			if (token != null)
			{
				AttachUserToContext(context, token);
			}

			// Continúa con el siguiente middleware en el pipeline
			await _next(context);
		}

		// Método que valida el token JWT y adjunta la información del usuario al contexto
		private void AttachUserToContext(HttpContext context, string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				// Obtiene la clave secreta usada para firmar el token
				var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

				// Configuración de los parámetros de validación del token
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true, // Valida la clave de firma del token
					IssuerSigningKey = new SymmetricSecurityKey(key), // Clave utilizada para validar la firma
					ValidateIssuer = true, // Valida el emisor del token
					ValidateAudience = true, // Valida el público del token
					ValidIssuer = _configuration["Jwt:Issuer"], // Emisor permitido configurado
					ValidAudience = _configuration["Jwt:Audience"], // Público permitido configurado
					ValidateLifetime = true, // Valida la caducidad del token
					ClockSkew = TimeSpan.Zero // Elimina la tolerancia en la validación del tiempo
				}, out SecurityToken validatedToken);

				// Si la validación es exitosa, obtiene el token JWT validado
				var jwtToken = (JwtSecurityToken)validatedToken;
				// Extrae el "userId" del claim del token con tipo "nameid"
				var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;

				// Adjunta la identidad del usuario al contexto, para que esté disponible en los controladores
				context.Items["User"] = userId;

				// Registro informativo para indicar que el token fue validado correctamente
				_logger.LogInformation($"JWT token validado correctamente para el usuario: {userId}");
			}
			catch (Exception ex)
			{
				_logger.LogWarning($"Error en la validación del token JWT: {ex.Message}");
			}
		}
	}

	// Clase de extensión para agregar el middleware al pipeline de solicitudes HTTP
	public static class JwtMiddlewareExtensions
	{
		// Método de extensión para simplificar la configuración del middleware en el startup
		public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
		{
			// Registra el middleware en la cadena de procesamiento
			return builder.UseMiddleware<JwtMiddleware>();
		}
	}
}
