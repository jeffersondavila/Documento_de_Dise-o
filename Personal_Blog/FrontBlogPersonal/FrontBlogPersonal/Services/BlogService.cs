using FrontBlogPersonal.Models;
using Blazored.LocalStorage;
using System.Text.Json;

namespace FrontBlogPersonal.Services
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public BlogService(HttpClient client, ILocalStorageService localStorage)
        {
            httpClient = client;
            this.localStorage = localStorage;
            jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora diferencias de mayúsculas/minúsculas en los nombres de propiedades
            };
        }

        public async Task<List<BlogModel>?> GetBlogs(int pageNumber, int pageSize)
        {
            try
            {
                // Construir la URL con parámetros
                var url = $"api/Blog?pageNumber={pageNumber}&pageSize={pageSize}";

                // Recuperar el token del almacenamiento local
                var token = await localStorage.GetItemAsync<string>("authToken");

                // Configurar el encabezado de autorización solo si el token existe
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    // Si el token no existe, asegúrate de no enviar el encabezado de autorización
                    httpClient.DefaultRequestHeaders.Authorization = null;
                }

                // Realizar la solicitud GET
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Leer y deserializar la respuesta
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<BlogModel>>(json, jsonSerializerOptions);
                }

                Console.WriteLine($"Error al obtener los blogs: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener los blogs: {ex.Message}");
                return null;
            }
        }

        public async Task<BlogModel?> GetBlogById(int codigoBlog)
        {
            try
            {
                // Recuperar el token del almacenamiento local
                var token = await localStorage.GetItemAsync<string>("authToken");

                // Verificar si el token está presente
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No se encontró un token válido en el almacenamiento local. La solicitud no se realizará.");
                    return null; // O lanzar una excepción, dependiendo de cómo manejes los errores en tu aplicación
                }

                // Configurar el encabezado de autorización
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Construir la URL
                var url = $"api/Blog/{codigoBlog}";

                // Realizar la solicitud GET
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Leer y deserializar la respuesta
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<BlogModel>(json, jsonSerializerOptions);
                }

                // Manejar errores de la solicitud
                Console.WriteLine($"Error al obtener el blog con ID {codigoBlog}: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener el blog con ID {codigoBlog}: {ex.Message}");
                return null;
            }
        }
    }

    public interface IBlogService
    {
        Task<List<BlogModel>?> GetBlogs(int pageNumber, int pageSize);
        Task<BlogModel?> GetBlogById(int codigoBlog);
    }
}
