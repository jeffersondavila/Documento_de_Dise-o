using BlogPersonal.Models;
using BlogPersonal.Services;
using BlogPersonal.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtener la clave JWT desde las variables de entorno o desde el archivo de configuración.
var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["Jwt:Key"]!);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger solo en desarrollo
if (builder.Environment.IsDevelopment())
{
	builder.Services.AddSwaggerGen(c =>
	{
		c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de Blog Personal", Version = "v1" });

		// Definir el esquema de seguridad para Swagger que permita el uso de JWT
		c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		{
			Name = "Authorization",
			Type = SecuritySchemeType.Http,
			Scheme = "Bearer",
			BearerFormat = "JWT",
			In = ParameterLocation.Header,
			Description = "Introduce el token JWT de esta manera: Bearer {tu token}"
		});

		c.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				},
				Array.Empty<string>()
			}
		});
	});
}

// Configurar la autenticación JWT (necesario para los esquemas y desafíos predeterminados)
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ClockSkew = TimeSpan.Zero, // Elimina la tolerancia de tiempo
		RequireExpirationTime = true // Asegura que el token siempre tenga un tiempo de expiración
	};
});

// Configurar los servicios personalizados (DbContext y Servicios)
builder.Services.AddSqlServer<PersonalBlogContext>(builder.Configuration.GetConnectionString("dbConnection"));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();

// Manejar excepciones de manera diferente en desarrollo y producción
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Blog Personal v1");
		c.RoutePrefix = "swagger";
	});
	// Mostrar página de excepción detallada en desarrollo
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

// Invocar el middleware JWT para la validación de tokens
app.UseMiddleware<JwtMiddleware>();

// Mantener la autenticación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
