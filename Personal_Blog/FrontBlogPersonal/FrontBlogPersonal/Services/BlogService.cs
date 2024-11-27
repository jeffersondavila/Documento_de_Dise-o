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

                // Realizar la solicitud GET
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
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
                // Configurar encabezado de autorización
                await SetAuthorizationHeader();

                // Construir la URL
                var url = $"api/Blog/{codigoBlog}";

                // Realizar la solicitud GET
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<BlogModel>(json, jsonSerializerOptions);
                }

                Console.WriteLine($"Error al obtener el blog con ID {codigoBlog}: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al obtener el blog con ID {codigoBlog}: {ex.Message}");
                return null;
            }
        }

        public async Task<int?> SaveBlog(BlogModel blog)
        {
            try
            {
                // Configurar encabezado de autorización
                await SetAuthorizationHeader();

                // Serializar el objeto
                var jsonContent = new StringContent(JsonSerializer.Serialize(blog, jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json");

                // Realizar la solicitud POST
                var response = await httpClient.PostAsync("api/Blog", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<int>(json, jsonSerializerOptions);
                }

                Console.WriteLine($"Error al guardar el blog: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al guardar el blog: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBlog(int codigoBlog, BlogModel blog)
        {
            try
            {
                // Configurar encabezado de autorización
                await SetAuthorizationHeader();

                // Serializar el objeto
                var jsonContent = new StringContent(JsonSerializer.Serialize(blog, jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json");

                // Realizar la solicitud PUT
                var response = await httpClient.PutAsync($"api/Blog/{codigoBlog}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                Console.WriteLine($"Error al actualizar el blog con ID {codigoBlog}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al actualizar el blog con ID {codigoBlog}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteBlog(int codigoBlog)
        {
            try
            {
                // Configurar encabezado de autorización
                await SetAuthorizationHeader();

                // Realizar la solicitud DELETE
                var response = await httpClient.DeleteAsync($"api/Blog/{codigoBlog}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                Console.WriteLine($"Error al eliminar el blog con ID {codigoBlog}: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al eliminar el blog con ID {codigoBlog}: {ex.Message}");
                return false;
            }
        }

        private async Task SetAuthorizationHeader()
        {
            var token = await localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                throw new UnauthorizedAccessException("No se encontró un token válido en el almacenamiento local.");
            }
        }
    }

    public interface IBlogService
    {
        Task<List<BlogModel>?> GetBlogs(int pageNumber, int pageSize);
        Task<BlogModel?> GetBlogById(int codigoBlog);
        Task<int?> SaveBlog(BlogModel blog);
        Task<bool> UpdateBlog(int codigoBlog, BlogModel blog);
        Task<bool> DeleteBlog(int codigoBlog);
    }
}
