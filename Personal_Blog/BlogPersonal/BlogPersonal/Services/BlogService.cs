﻿using BlogPersonal.DTO;
using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogPersonal.Services
{
	public class BlogService : IBlogService
	{
		private readonly PersonalBlogContext _blogContext;
		private readonly ILogger<BlogService> _logger;

		public BlogService(PersonalBlogContext blogContext, ILogger<BlogService> logger)
		{
			_blogContext = blogContext;
			_logger = logger;
		}

		private async Task ValidarUsuarioYEstado(int codigoUsuario, int codigoEstadoBlog)
		{
			var usuarioExiste = await _blogContext.Usuarios.AnyAsync(u => u.CodigoUsuario == codigoUsuario);
			var estadoBlogExiste = await _blogContext.EstadoBlogs.AnyAsync(e => e.CodigoEstadoBlog == codigoEstadoBlog);

			if (!usuarioExiste)
			{
				throw new ArgumentException("El usuario proporcionado no existe.");
			}

			if (!estadoBlogExiste)
			{
				throw new ArgumentException("El estado del blog proporcionado no existe.");
			}
		}

		public async Task<IEnumerable<BlogDto?>> GetAllBlog(int pageNumber = 1, int pageSize = 10)
		{
			// Obtiene blogs paginados, numero de pagina de inicio y cantidad de blogs
			var blogs = await _blogContext.Blogs
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.Select(x => new BlogDto
				{
					CodigoBlog = x.CodigoBlog,
					Titulo = x.Titulo,
					Contenido = x.Contenido,
					FechaCreacion = x.FechaCreacion,
					FechaModificacion = x.FechaModificacion,
					CodigoUsuario = x.CodigoUsuario,
					CodigoEstadoBlog = x.CodigoEstadoBlog
				})
				.ToListAsync();

			if(blogs == null)
			{
				return null!;
			}

			return blogs;
		}

		public async Task<BlogDto?> GetBlog(int id)
		{
			// Obtiene un blog en especifico
			var blog = await _blogContext.Blogs
				.Where(b => b.CodigoBlog == id)
				.Select(x => new BlogDto
				{
					CodigoBlog = x.CodigoBlog,
					Titulo = x.Titulo,
					Contenido = x.Contenido,
					FechaCreacion = x.FechaCreacion,
					FechaModificacion = x.FechaModificacion,
					CodigoUsuario = x.CodigoUsuario,
					CodigoEstadoBlog = x.CodigoEstadoBlog
				})
				.FirstOrDefaultAsync();

			if (blog == null)
			{
				return null;
			}

			return blog;
		}

		public async Task<int> SaveBlog(BlogDto blogDto, int userId)
		{
			if (userId != blogDto.CodigoUsuario)
			{
				throw new UnauthorizedAccessException("No tienes permisos para crear un blog para este usuario.");
			}

			// Verifica si existe la informacion del mantenimiento
			await ValidarUsuarioYEstado(blogDto.CodigoUsuario, blogDto.CodigoEstadoBlog);

			var blog = new Blog
			{
				Titulo = blogDto.Titulo ?? "",
				Contenido = blogDto.Contenido ?? "",
				FechaCreacion = blogDto.FechaCreacion,
				FechaModificacion = blogDto.FechaModificacion,
				CodigoUsuario = blogDto.CodigoUsuario,
				CodigoEstadoBlog = blogDto.CodigoEstadoBlog
			};

			try
			{
				// Registra blog en la bd
				_blogContext.Blogs.Add(blog);
				await _blogContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error guardando blog: {ex.Message}");
				throw new Exception("Error interno al guardar el blog.");
			}

			return blog.CodigoBlog;
		}

		public async Task UpdateBlog(BlogDto blogDto, int id, int userId)
		{
			// Busca blog por medio de su codigo
			var blogActual = await _blogContext.Blogs.FirstOrDefaultAsync(b => b.CodigoBlog == id);

			if (blogActual == null)
			{
				throw new KeyNotFoundException("El blog no fue encontrado.");
			}

			//if (blogActual.CodigoUsuario != userId)
			//{
			//	throw new UnauthorizedAccessException("No tienes permiso para actualizar este blog.");
			//}

			// Asigna nuevos valores a los campos
			blogActual.Titulo = blogDto.Titulo ?? "";
			blogActual.Contenido = blogDto.Contenido ?? "";
			blogActual.FechaModificacion = blogDto.FechaModificacion ?? DateTime.Now;
			blogActual.CodigoEstadoBlog = blogDto.CodigoEstadoBlog;

			try
			{
				// Modifica blog en la bd
				await _blogContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error actualizando blog con ID {id}: {ex.Message}");
				throw new Exception("Error interno al actualizar el blog.");
			}
		}

		public async Task DeleteBlog(int id, int userId)
		{
			// Busca blog por medio de su codigo
			var blogActual = await _blogContext.Blogs.FirstOrDefaultAsync(b => b.CodigoBlog == id);

			if (blogActual == null)
			{
				throw new KeyNotFoundException("El blog no fue encontrado.");
			}

			//if (blogActual.CodigoUsuario != userId)
			//{
			//	throw new UnauthorizedAccessException("No tienes permiso para eliminar este blog.");
			//}

			try
			{
				// Elimina el blog de la bd
				_blogContext.Blogs.Remove(blogActual);
				await _blogContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error eliminando blog con ID {id}: {ex.Message}");
				throw new Exception("Error interno al eliminar el blog.");
			}
		}
	}

	public interface IBlogService
	{
		Task<IEnumerable<BlogDto?>> GetAllBlog(int pageNumber = 1, int pageSize = 10);
		Task<BlogDto?> GetBlog(int id);
		Task<int> SaveBlog(BlogDto blogDto, int userId);
		Task UpdateBlog(BlogDto blogDto, int id, int userId);
		Task DeleteBlog(int id, int userId);
	}
}
