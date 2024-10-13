using BlogPersonal.Models;
using BlogPersonal.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Obtener la clave JWT desde las variables de entorno o desde el archivo de configuraci�n.
var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? builder.Configuration["Jwt:Key"]!);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger solo en desarrollo
if (builder.Environment.IsDevelopment())
{
	builder.Services.AddSwaggerGen();
}

// Configurar la autenticaci�n JWT
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
		RequireExpirationTime = true // Asegura que el token siempre tenga un tiempo de expiraci�n
	};

	options.Events = new JwtBearerEvents
	{
		OnAuthenticationFailed = context =>
		{
			if (builder.Environment.IsDevelopment())
			{
				// En desarrollo, puedes escribir mensajes detallados sobre la falla de autenticaci�n
				Console.WriteLine("Token inv�lido: " + context.Exception.Message);
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
			// En desarrollo o producci�n, puedes mostrar la raz�n del desaf�o JWT fallido
			Console.WriteLine("Desaf�o JWT fallido: " + context.ErrorDescription);
			return Task.CompletedTask;
		}
	};
});

// Configurar los servicios personalizados (DbContext y Servicios)
builder.Services.AddSqlServer<PersonalBlogContext>(builder.Configuration.GetConnectionString("dbConnection"));
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IBlogService, BlogService>();

var app = builder.Build();

// Manejar excepciones de manera diferente en desarrollo y producci�n
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseDeveloperExceptionPage(); // Mostrar p�gina de excepci�n detallada en desarrollo
}
else
{
	app.UseExceptionHandler("/Home/Error"); // P�gina gen�rica de errores en producci�n
	app.UseHsts(); // Seguridad para forzar HTTPS en producci�n
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Agregar autenticaci�n
app.UseAuthorization();

app.MapControllers();

app.Run();
