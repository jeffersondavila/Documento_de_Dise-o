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

		public async Task Invoke(HttpContext context)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (token != null)
			{
				AttachUserToContext(context, token);
			}

			await _next(context); // Pasa la solicitud al siguiente middleware en el pipeline
		}

		private void AttachUserToContext(HttpContext context, string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = _configuration["Jwt:Issuer"],
					ValidAudience = _configuration["Jwt:Audience"],
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero // Elimina la tolerancia de tiempo
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = jwtToken.Claims.First(x => x.Type == "nameid").Value;

				// Adjuntar la identidad del usuario al contexto para su uso en los controladores
				context.Items["User"] = userId;

				_logger.LogInformation($"JWT token validado correctamente para el usuario: {userId}");
			}
			catch (Exception ex)
			{
				// Si la validación falla, no hacer nada y registrar el error
				_logger.LogWarning($"Error en la validación del token JWT: {ex.Message}");
			}
		}
	}

	// Método de extensión para agregar el middleware al pipeline de solicitudes HTTP
	public static class JwtMiddlewareExtensions
	{
		public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<JwtMiddleware>();
		}
	}
}
