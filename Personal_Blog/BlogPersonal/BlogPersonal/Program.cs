using BlogPersonal.Models;
using BlogPersonal.Services;
using BlogPersonal.Middlewares;
using BlogPersonal.Extensions; // Importar las extensiones personalizadas

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation(); // Agregar documentaci�n de Swagger
builder.Services.AddJwtAuthentication(builder.Configuration); // Agregar autenticaci�n JWT
builder.Services.AddSqlServer<PersonalBlogContext>(builder.Configuration.GetConnectionString("dbConnection"));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddCustomCors(builder.Configuration); // Agregar la pol�tica CORS personalizada

var app = builder.Build();

app.UseCustomExceptionHandling(app.Environment); // Configurar middleware de excepciones
app.UseCustomCors(); // Aplicar middleware CORS personalizado
app.UseMiddleware<JwtMiddleware>(); // Invocar el middleware JWT para la validaci�n de tokens

// Mantener la autenticaci�n y autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
