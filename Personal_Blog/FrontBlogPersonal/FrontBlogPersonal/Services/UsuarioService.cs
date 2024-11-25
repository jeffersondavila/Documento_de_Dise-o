using FrontBlogPersonal.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace FrontBlogPersonal.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public UsuarioService(HttpClient client)
        {
            httpClient = client;
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora diferencias de mayúsculas/minúsculas en los nombres de propiedades
            };
        }

        /// <summary>
        /// Envía las credenciales de login y obtiene un token y la información del usuario.
        /// </summary>
        /// <param name="correo">Correo electrónico del usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>Un UsuarioLoginResponseModel si el login es exitoso; null si falla.</returns>
        public async Task<UsuarioLoginResponseModel?> LoginAsync(string correo, string password)
        {
            try
            {
                var loginData = new UsuarioLoginModel
                {
                    Correo = correo,
                    Password = password
                };

                // Realizar la solicitud POST al endpoint de login
                var response = await httpClient.PostAsJsonAsync("api/Usuario/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    // Leer y deserializar la respuesta en UsuarioLoginResponseModel
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UsuarioLoginResponseModel>(json, jsonSerializerOptions);
                }

                // Manejar errores de respuesta (opcional: registrar el código de estado)
                Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (opcional: registrar el mensaje de error)
                Console.WriteLine($"Error al intentar iniciar sesión: {ex.Message}");
                return null;
            }
        }
    }

    public interface IUsuarioService
    {
        Task<UsuarioLoginResponseModel?> LoginAsync(string correo, string password);
    }
}
