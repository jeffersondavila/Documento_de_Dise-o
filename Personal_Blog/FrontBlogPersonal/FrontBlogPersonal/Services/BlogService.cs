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

        public async Task<List<BlogModel>?> GetBlogsAsync(int pageNumber, int pageSize)
        {
            try
            {
                // Recuperar el token del almacenamiento local
                var token = await localStorage.GetItemAsync<string>("authToken");

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("No se encontró el token en el almacenamiento local.");
                    return null;
                }

                // Configurar el encabezado de autorización
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Construir la URL con parámetros
                var url = $"api/Blog?pageNumber={pageNumber}&pageSize={pageSize}";

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
    }

    public interface IBlogService
    {
        Task<List<BlogModel>?> GetBlogsAsync(int pageNumber, int pageSize);
    }
}
