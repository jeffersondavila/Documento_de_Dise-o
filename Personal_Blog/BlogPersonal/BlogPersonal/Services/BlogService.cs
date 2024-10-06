using BlogPersonal.DTO;
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
							CodigoUsuario = x.CodigoUsuario,
							CodigoEstadoBlog = x.CodigoEstadoBlog
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
							CodigoUsuario = x.CodigoUsuario,
							CodigoEstadoBlog = x.CodigoEstadoBlog
						})
						.FirstOrDefaultAsync();

			return blog;
		}

		public async Task<int> SaveBlog(BlogDto blogDto)
		{
			var blog = new Blog
			{
				Titulo = blogDto.Titulo ?? "",
				Contenido = blogDto.Contenido ?? "",
				FechaCreacion = blogDto.FechaCreacion,
				FechaModificacion = blogDto.FechaModificacion,
				CodigoUsuario = blogDto.CodigoUsuario,
				CodigoEstadoBlog = blogDto.CodigoEstadoBlog
			};

			_blogContext.Blogs.Add(blog);
			await _blogContext.SaveChangesAsync();

			return blog.CodigoBlog;
		}

		public async Task UpdateBlog(BlogDto blogDto, int id)
		{
			var blogActual = await _blogContext.Blogs.FirstOrDefaultAsync(b => b.CodigoBlog == id);

			if (blogActual != null)
			{
				blogActual.Titulo = blogDto.Titulo ?? "" ;
				blogActual.Contenido = blogDto.Contenido ?? "";
				blogActual.FechaModificacion = blogDto.FechaModificacion;
				blogActual.CodigoUsuario= blogDto.CodigoUsuario;
				blogActual.CodigoEstadoBlog = blogDto.CodigoEstadoBlog;
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
		Task<int> SaveBlog(BlogDto blogDto);
		Task UpdateBlog(BlogDto blogDto, int id);
		Task DeleteBlog(int id);
	}
}
