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

		// POST api/Usuario/sigup
		[HttpPost("sigup")]
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

			try
			{
				var result = await _usuarioService.RegistrarUsuario(usuario);

				if (!result)
				{
					return Conflict("El usuario ya existe.");
				}

				return Ok("Usuario registrado exitosamente.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error interno: {ex.Message}");
			}
		}

		// POST api/Usuario/login
		[HttpPost("login")]
		public async Task<IActionResult> Ingreso([FromBody] UsuarioLoginDto usuarioDto)
		{
			if (usuarioDto == null || string.IsNullOrEmpty(usuarioDto.Correo) || string.IsNullOrEmpty(usuarioDto.Password))
			{
				return BadRequest("Correo o contraseña no proporcionados.");
			}

			// Obtener el token y la información del usuario
			var loginResponse = await _usuarioService.IngresoUsuario(usuarioDto.Correo, usuarioDto.Password);

			if (loginResponse == null)
			{
				return Unauthorized("Correo o contraseña incorrectos.");
			}

			// Retornar tanto el token como la información del usuario
			return Ok(loginResponse);
		}

		// POST api/Usuario/recuperar-password
		[HttpPost("recuperar-password")]
		public async Task<IActionResult> SolicitarRecuperacionPassword([FromBody] UsuarioRecuperarPasswordDto recuperarPasswordDto)
		{
			if (recuperarPasswordDto == null || string.IsNullOrEmpty(recuperarPasswordDto.Correo))
			{
				return BadRequest("El correo es requerido.");
			}

			var result = await _usuarioService.SolicitarRecuperacionPassword(recuperarPasswordDto.Correo);

			if (!result)
			{
				return NotFound("El correo no se encuentra registrado.");
			}

			return Ok("Se ha enviado un correo con las instrucciones para recuperar la contraseña.");
		}

		// POST api/Usuario/restablecer-password
		[HttpPost("restablecer-password")]
		public async Task<IActionResult> RestablecerPassword([FromBody] UsuarioRestablecerPasswordDto restablecerPasswordDto)
		{
			if (restablecerPasswordDto == null || string.IsNullOrEmpty(restablecerPasswordDto.TokenRecuperacion) || string.IsNullOrEmpty(restablecerPasswordDto.NuevaPassword))
			{
				return BadRequest("Los datos proporcionados son inválidos.");
			}

			var result = await _usuarioService.RestablecerPassword(restablecerPasswordDto.TokenRecuperacion, restablecerPasswordDto.NuevaPassword);

			if (!result)
			{
				return NotFound("El token de recuperación es inválido o ha expirado.");
			}

			return Ok("Contraseña restablecida correctamente.");
		}
	}
}
