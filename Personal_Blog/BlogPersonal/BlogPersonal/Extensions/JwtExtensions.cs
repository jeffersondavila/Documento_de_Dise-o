using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogPersonal.Extensions
{
	public static class JwtExtensions
	{
		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

			// Configurar servicio de autenticación JWT en la aplicación
			services.AddAuthentication(options =>
			{
				// Establecer esquema de autenticación predeterminado como JWT Bearer
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			// Configurar manejo específico de la autenticación JWT
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true, // Verifica que el emisor del token sea válido
					ValidateAudience = true, // Verifica que el público del token sea válido
					ValidateLifetime = true, // Verifica que el token no haya expirado
					ValidateIssuerSigningKey = true, // Verifica la clave de firma del emisor
					ValidIssuer = configuration["Jwt:Issuer"], // Emisor válido del token
					ValidAudience = configuration["Jwt:Audience"], // Público válido del token
					IssuerSigningKey = new SymmetricSecurityKey(key), // Llave utilizada para firmar el token
					ClockSkew = TimeSpan.Zero, // Elimina la tolerancia de tiempo para expirar del token
					RequireExpirationTime = true // Requiere que el token tenga un tiempo de expiración establecido
				};
			});

			// Retorna IServiceCollection para permitir la cadena de configuraciones
			return services;
		}
	}
}
