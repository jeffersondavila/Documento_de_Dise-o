using BlogPersonal.DTO;
using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogPersonal.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly IBlogService _blogService;

		public BlogController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		// GET: api/Blog
		[HttpGet]
		public async Task<IActionResult> GetAllBlog()
		{
			var blogs = await _blogService.GetAllBlog();
			return Ok(blogs);
		}

		// GET: api/Blog/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetBlog(int id)
		{
			var blog = await _blogService.GetBlog(id);

			if (blog == null)
			{
				return NotFound();
			}

			return Ok(blog);
		}

		// POST: api/Blog
		[HttpPost]
		public async Task<IActionResult> SaveBlog([FromBody] BlogDto blog)
		{
			if (blog == null || string.IsNullOrEmpty(blog.Titulo) || string.IsNullOrEmpty(blog.Contenido))
			{
				return BadRequest("El blog o los datos requeridos están vacíos.");
			}

			var codigoBlog = await _blogService.SaveBlog(blog);
			var savedBlog = await _blogService.GetBlog(codigoBlog);

			return CreatedAtAction(nameof(GetBlog), new { id = savedBlog!.CodigoBlog }, savedBlog);
		}

		// PUT: api/Blog/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogDto blog)
		{
			if (blog == null)
			{
				return BadRequest("Los datos del blog son inválidos.");
			}

			var blogActual = await _blogService.GetBlog(id);
			if (blogActual == null)
			{
				return NotFound("El blog no se encontró.");
			}

			await _blogService.UpdateBlog(blog, id);
			return NoContent();
		}


		// DELETE: api/Blog/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlog(int id)
		{
			var blog = await _blogService.GetBlog(id);
			if (blog == null)
			{
				return NotFound("El blog no se encontró.");
			}

			await _blogService.DeleteBlog(id);
			return NoContent();
		}

	}
}
