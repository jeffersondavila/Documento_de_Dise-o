using BlogPersonal.DTO;
using BlogPersonal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogPersonal.Services
{
	public class UsuarioService : IUsuarioService
	{
		private readonly PersonalBlogContext _blogContext;
		private readonly PasswordHasher<Usuario> _passwordHasher;
		private readonly IConfiguration _configuration;

		public UsuarioService(PersonalBlogContext blogContext, IConfiguration configuration)
		{
			_blogContext = blogContext;
			_passwordHasher = new PasswordHasher<Usuario>();
			_configuration = configuration;
		}

		public async Task<bool> RegistrarUsuario(Usuario usuario)
		{
			var usuarioActual = await _blogContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == usuario.Correo);

			if (usuarioActual != null)
			{
				return false;
			}

			usuario.Password = _passwordHasher.HashPassword(usuario, usuario.Password);

			_blogContext.Usuarios.Add(usuario);
			await _blogContext.SaveChangesAsync();

			return true;
		}

		private string GenerateJwtToken(Usuario usuario)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, usuario.CodigoUsuario.ToString()),
					new Claim(ClaimTypes.Name, usuario.Correo)
				}),
				Expires = DateTime.UtcNow.AddHours(1), // El token expira en 1 hora
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public async Task<UsuarioLoginResponseDto?> IngresoUsuario(string correo, string password)
		{
			var usuarioActual = await _blogContext.Usuarios
				.Include(u => u.Blogs)
				.FirstOrDefaultAsync(u => u.Correo == correo);

			if (usuarioActual != null && VerificarPassword(usuarioActual, password))
			{
				usuarioActual.FechaUltimoAcceso = DateTime.Now;
				await _blogContext.SaveChangesAsync();

				// Generar el token JWT
				var token = GenerateJwtToken(usuarioActual);

				// Devolver el token y la información del usuario
				return new UsuarioLoginResponseDto
				{
					Token = token,
					Usuario = new UsuarioDto
					{
						CodigoUsuario = usuarioActual.CodigoUsuario,
						Nombre = usuarioActual.Nombre,
						Correo = usuarioActual.Correo,
						CodigoEstadoUsuario = usuarioActual.CodigoEstadoUsuario,
						Blogs = usuarioActual.Blogs.Select(b => new BlogDto
						{
							CodigoBlog = b.CodigoBlog,
							Titulo = b.Titulo,
							Contenido = b.Contenido,
							FechaCreacion = b.FechaCreacion,
							FechaModificacion = b.FechaModificacion
						}).ToList()
					}
				};
			}

			return null;
		}

		private bool VerificarPassword(Usuario usuario, string passwordIngresada)
		{
			var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, passwordIngresada);
			return result == PasswordVerificationResult.Success;
		}

		public async Task<bool> SolicitarRecuperacionPassword(string correo)
		{
			var usuario = await _blogContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

			if (usuario == null)
			{
				return false;
			}

			usuario.TokenRecuperacion = Guid.NewGuid().ToString();
			await _blogContext.SaveChangesAsync();

			// Lógica para enviar correo con el token de recuperación

			return true;
		}

		public async Task<bool> RestablecerPassword(string tokenRecuperacion, string nuevaPassword)
		{
			var usuario = await _blogContext.Usuarios.FirstOrDefaultAsync(u => u.TokenRecuperacion == tokenRecuperacion);

			if (usuario == null)
			{
				return false;
			}

			usuario.Password = _passwordHasher.HashPassword(usuario, nuevaPassword);
			usuario.TokenRecuperacion = null;

			await _blogContext.SaveChangesAsync();

			return true;
		}
	}

	public interface IUsuarioService
	{
		Task<bool> RegistrarUsuario(Usuario usuario);
		Task<UsuarioLoginResponseDto?> IngresoUsuario(string correo, string password);
		Task<bool> SolicitarRecuperacionPassword(string correo);
		Task<bool> RestablecerPassword(string tokenRecuperacion, string nuevaPassword);
	}
}
