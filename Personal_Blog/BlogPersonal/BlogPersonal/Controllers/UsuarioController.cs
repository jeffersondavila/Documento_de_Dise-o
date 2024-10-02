using BlogPersonal.DTO;
using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogPersonal.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuarioController : ControllerBase
	{
		private readonly IUsuarioService _usuarioService;

		public UsuarioController(IUsuarioService service)
		{
			_usuarioService = service;
		}

		// POST api/Usuario
		[HttpPost]
		public async Task<IActionResult> Registrar([FromBody] UsuarioCreateDto usuarioDto)
		{
			if (usuarioDto == null || string.IsNullOrEmpty(usuarioDto.Correo) || string.IsNullOrEmpty(usuarioDto.Password))
			{
				return BadRequest("El usuario o los datos requeridos están vacíos.");
			}

			var usuario = new Usuario
			{
				Nombre = usuarioDto.Nombre ?? "",
				Correo = usuarioDto.Correo,
				Password = usuarioDto.Password,
				CodigoEstadoUsuario = 1
			};

			var result = await _usuarioService.RegistrarUsuario(usuario);

			if (!result)
			{
				return Conflict("El usuario ya existe.");
			}

			return Ok("Usuario registrado exitosamente.");
		}

		// POST api/Usuario/login
		[HttpPost("login")]
		public async Task<IActionResult> Ingreso([FromBody] UsuarioLoginDto usuarioDto)
		{
			if (usuarioDto == null || string.IsNullOrEmpty(usuarioDto.Correo) || string.IsNullOrEmpty(usuarioDto.Password))
			{
				return BadRequest("Correo o contraseña no proporcionados.");
			}

			var usuarioLogueado = await _usuarioService.IngresoUsuario(usuarioDto.Correo, usuarioDto.Password);

			if (usuarioLogueado == null)
			{
				return Unauthorized("Correo o contraseña incorrectos.");
			}

			return Ok(usuarioLogueado);
		}
	}
}
