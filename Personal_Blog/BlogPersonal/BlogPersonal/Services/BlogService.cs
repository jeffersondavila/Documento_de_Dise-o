﻿using BlogPersonal.DTO;
using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Services
{
	public class BlogService : IBlogService
	{
		private readonly PersonalBlogContext _blogContext;

		public BlogService(PersonalBlogContext blogContext)
		{
			_blogContext = blogContext;
		}

		public async Task <IEnumerable<BlogDto?>> GetAllBlog()
		{
			var blog = await _blogContext.Blogs
						.Select(x => new BlogDto
						{
							CodigoBlog = x.CodigoBlog,
							Titulo = x.Titulo,
							Contenido = x.Contenido,
							FechaCreacion = x.FechaCreacion,
							FechaModificacion = x.FechaModificacion,
							CodigoUsuario = x.CodigoUsuarioNavigation.CodigoUsuario,
							CodigoEstadoBlog = x.CodigoEstadoBlogNavigation.CodigoEstadoBlog
						})
						.ToListAsync();

			return blog;
		}

		public async Task<BlogDto?> GetBlog(int id)
		{
			var blog = await _blogContext.Blogs
						.Where(b => b.CodigoBlog == id)
						.Select(x => new BlogDto
						{
							CodigoBlog = x.CodigoBlog,
							Titulo = x.Titulo,
							Contenido = x.Contenido,
							FechaCreacion = x.FechaCreacion,
							FechaModificacion = x.FechaModificacion,
							CodigoUsuario = x.CodigoUsuarioNavigation.CodigoUsuario,
							CodigoEstadoBlog = x.CodigoEstadoBlogNavigation.CodigoEstadoBlog
						})
						.FirstOrDefaultAsync();

			return blog;
		}

		public async Task SaveBlog(Blog blog)
		{
			_blogContext.Blogs.Add(blog);
			await _blogContext.SaveChangesAsync();
		}

		public async Task UpdateBlog(Blog blog, int id)
		{
			var blogActual = await _blogContext.Blogs.FirstOrDefaultAsync(b => b.CodigoBlog == id);

			if (blogActual != null)
			{
				blogActual.Titulo = blog.Titulo;
				blogActual.Contenido = blog.Contenido;
				blogActual.FechaModificacion = blog.FechaModificacion;
				blogActual.CodigoEstadoBlog = blog.CodigoEstadoBlog;
				await _blogContext.SaveChangesAsync();
			}
		}

		public async Task DeleteBlog(int id)
		{
			var blogActual = await _blogContext.Blogs.FirstOrDefaultAsync(b => b.CodigoBlog == id);

			if (blogActual != null)
			{
				_blogContext.Remove(blogActual);
				await _blogContext.SaveChangesAsync();
			}
		}
	}

	public interface IBlogService
	{
		Task<IEnumerable<BlogDto?>> GetAllBlog();
		Task<BlogDto?> GetBlog(int id);
		Task SaveBlog(Blog blog);
		Task UpdateBlog(Blog blog, int id);
		Task DeleteBlog(int id);
	}
}
