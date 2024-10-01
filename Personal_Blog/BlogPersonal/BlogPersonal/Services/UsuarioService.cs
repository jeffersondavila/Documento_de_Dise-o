using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BlogPersonal.Services
{
	public class UsuarioService : IUsuarioService
	{
		private readonly PersonalBlogContext _blogContext;

		public UsuarioService(PersonalBlogContext blogContext)
		{
			_blogContext = blogContext;
		}

		public async Task<bool> RegistrarUsuario(Usuario usuario)
		{
			var usuarioActual = await _blogContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == usuario.Correo);

			if (usuarioActual != null)
			{
				return false;
			}

			usuario.Password = HashPassword(usuario.Password);

			_blogContext.Usuarios.Add(usuario);
			await _blogContext.SaveChangesAsync();

			return true;
		}

		public async Task<Usuario?> IngresoUsuario(string correo, string password)
		{
			var usuarioActual = await _blogContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

			if (usuarioActual != null && VerificarPassword(password, usuarioActual.Password))
			{
				return usuarioActual;
			}

			return null;
		}

		private string HashPassword(string password)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}

		private bool VerificarPassword(string passwordIngresada, string passwordAlmacenada)
		{
			var hashedPassword = HashPassword(passwordIngresada);
			return hashedPassword == passwordAlmacenada;
		}
	}

	public interface IUsuarioService
	{
		Task<bool> RegistrarUsuario(Usuario usuario);
		Task<Usuario?> IngresoUsuario(string correo, string password);
	}
}
