using BlogPersonal.Models;
using BlogPersonal.Services;
using BlogPersonal.Middlewares;
using BlogPersonal.Extensions; // Importar las extensiones personalizadas

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation(); // Agregar documentación de Swagger
builder.Services.AddJwtAuthentication(builder.Configuration); // Agregar autenticación JWT
builder.Services.AddSqlServer<PersonalBlogContext>(builder.Configuration.GetConnectionString("dbConnection"));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();

// Configurar middleware de excepciones
app.UseCustomExceptionHandling(app.Environment);

// Invocar el middleware JWT para la validación de tokens
app.UseMiddleware<JwtMiddleware>();

// Mantener la autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
