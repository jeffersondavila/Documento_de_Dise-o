using BlogPersonal.DTO;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogPersonal.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class BlogController : ControllerBase
	{
		private readonly IBlogService _blogService;

		public BlogController(IBlogService blogService)
		{
			_blogService = blogService;
		}

		// GET: api/Blog?pageNumber=1&pageSize=10
		[HttpGet]
		public async Task<IActionResult> GetAllBlog([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
		{
			var blogs = await _blogService.GetAllBlog(pageNumber, pageSize);
			return Ok(blogs);
		}

		// GET: api/Blog/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetBlog(int id)
		{
			var blog = await _blogService.GetBlog(id);

			if (blog == null)
			{
				return NotFound("El blog no se encontró.");
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

			// Obtener el userId del JWT Token
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

			try
			{
				var codigoBlog = await _blogService.SaveBlog(blog, userId);
				var savedBlog = await _blogService.GetBlog(codigoBlog);

				return CreatedAtAction(nameof(GetBlog), new { id = savedBlog!.CodigoBlog }, savedBlog);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message); // 403 Forbidden
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error interno: {ex.Message}");
			}
		}

		// PUT: api/Blog/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBlog(int id, [FromBody] BlogDto blog)
		{
			if (blog == null)
			{
				return BadRequest("Los datos del blog son inválidos.");
			}

			// Obtener el userId del JWT Token
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

			try
			{
				await _blogService.UpdateBlog(blog, id, userId);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message); // 403 Forbidden
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error interno: {ex.Message}");
			}
		}

		// DELETE: api/Blog/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlog(int id)
		{
			// Obtener el userId del JWT Token
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

			try
			{
				await _blogService.DeleteBlog(id, userId);
				return NoContent();
			}
			catch (KeyNotFoundException ex)
			{
				return NotFound(ex.Message);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Forbid(ex.Message); // 403 Forbidden
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error interno: {ex.Message}");
			}
		}
	}
}
