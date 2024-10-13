using BlogPersonal.Models;
using BlogPersonal.Services;
using BlogPersonal.Middlewares;
using BlogPersonal.Extensions; // Importar las extensiones personalizadas

var builder = WebApplication.CreateBuilder(args);

// Se agregan servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Se agrega Swagger al entorno de desarrollo
if (builder.Environment.IsDevelopment())
{
	builder.Services.AddSwaggerDocumentation();
}

// Configurar la autenticación JWT desde las extensiones
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configurar CORS desde las extensiones
builder.Services.AddCustomCors(builder.Configuration);

// Configurar los servicios personalizados (DbContext y Servicios)
builder.Services.AddSqlServer<PersonalBlogContext>(builder.Configuration.GetConnectionString("dbConnection"));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();

// Configurar middleware de excepciones y swagger solo en desarrollo
app.UseCustomExceptionHandling(app.Environment);

// Invocar el middleware JWT para la validación de tokens
app.UseMiddleware<JwtMiddleware>();

// Habilitar CORS en la aplicación
app.UseCustomCors();

// Mantener la autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
