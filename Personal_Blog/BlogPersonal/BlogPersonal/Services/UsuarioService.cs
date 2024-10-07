using BlogPersonal.DTO;
using BlogPersonal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Services
{
	public class UsuarioService : IUsuarioService
	{
		private readonly PersonalBlogContext _blogContext;
		private readonly PasswordHasher<Usuario> _passwordHasher;

		public UsuarioService(PersonalBlogContext blogContext)
		{
			_blogContext = blogContext;
			_passwordHasher = new PasswordHasher<Usuario>();
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

		public async Task<UsuarioDto?> IngresoUsuario(string correo, string password)
		{
			var usuarioActual = await _blogContext.Usuarios
				.Include(u => u.Blogs)
				.FirstOrDefaultAsync(u => u.Correo == correo);

			if (usuarioActual != null && VerificarPassword(usuarioActual, password))
			{
				usuarioActual.FechaUltimoAcceso = DateTime.Now;
				await _blogContext.SaveChangesAsync();

				return new UsuarioDto
				{
					CodigoUsuario = usuarioActual.CodigoUsuario,
					Nombre = usuarioActual.Nombre,
					Correo = usuarioActual.Correo,
					CodigoEstadoUsuario = usuarioActual.CodigoEstadoUsuario,
					FechaUltimoAcceso = usuarioActual.FechaUltimoAcceso,
					Blogs = usuarioActual.Blogs.Select(b => new BlogDto
					{
						CodigoBlog = b.CodigoBlog,
						Titulo = b.Titulo,
						Contenido = b.Contenido,
						FechaCreacion = b.FechaCreacion,
						FechaModificacion = b.FechaModificacion
					}).ToList()
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

			// Logica enviar correo con el token de recuperacion

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
		Task<UsuarioDto?> IngresoUsuario(string correo, string password);
		Task<bool> SolicitarRecuperacionPassword(string correo);
		Task<bool> RestablecerPassword(string tokenRecuperacion, string nuevaPassword);
	}
}
