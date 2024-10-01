using BlogPersonal.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPersonal.Services
{
	public class BlogService : IBlogService
	{
		PersonalBlogContext _blogContext;

		public BlogService(PersonalBlogContext blogContext)
		{
			_blogContext = blogContext;
		}

		public async Task <IEnumerable<Blog>> GetAllBlog()
		{
			return await _blogContext.Blogs.ToListAsync();
		}

		public async Task<Blog?> GetBlog(int id)
		{
			return await _blogContext.Blogs.FirstOrDefaultAsync(b => b.CodigoBlog == id);
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
		Task<IEnumerable<Blog>> GetAllBlog();
		Task<Blog?> GetBlog(int id);
		Task SaveBlog(Blog blog);
		Task UpdateBlog(Blog blog, int id);
		Task DeleteBlog(int id);
	}
}
