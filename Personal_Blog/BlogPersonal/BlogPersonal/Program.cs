using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
	builder.Services.AddSwaggerGen();
}

// Configurar la autenticación JWT
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

	options.Events = new JwtBearerEvents
	{
		OnAuthenticationFailed = context =>
		{
			if (builder.Environment.IsDevelopment())
			{
				// En desarrollo, puedes escribir mensajes detallados sobre la falla de autenticación
				Console.WriteLine("Token inválido: " + context.Exception.Message);
			}
			return Task.CompletedTask;
		},
		OnTokenValidated = context =>
		{
			Console.WriteLine("Token validado correctamente");
			return Task.CompletedTask;
		},
		OnChallenge = context =>
		{
			// En desarrollo o producción, puedes mostrar la razón del desafío JWT fallido
			Console.WriteLine("Desafío JWT fallido: " + context.ErrorDescription);
			return Task.CompletedTask;
		}
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
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage(); // Mostrar página de excepción detallada en desarrollo
}
else
{
	app.UseExceptionHandler("/Home/Error"); // Página genérica de errores en producción
	app.UseHsts(); // Seguridad para forzar HTTPS en producción
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Agregar autenticación
app.UseAuthorization();

app.MapControllers();

app.Run();
