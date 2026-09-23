using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using GestorVideojuegos.Application.Interfaces;
using GestorVideojuegos.Application.Validations;
using GestorVideojuegos.Infrastructure.Persistence;
using GestorVideojuegos.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

// 1. Cargar las variables de entorno para no usar appsettings.json
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// 2. Obtener la cadena de conexión de forma segura
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La variable DB_CONNECTION_STRING no está en el .env");
}

// 3. Configurar el DbContext (Base de datos)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 4. Inyección de Dependencias del Repositorio (Clean Architecture)
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GestorVideojuegos.Application.Services.GameService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, GestorVideojuegos.Application.Services.AuthService>();
// 5. Agregar soporte para Controladores
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // La URL exacta de tu Vue
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ¡ESTO ES VITAL PARA ACEPTAR LAS COOKIES!
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        // Magia: Le decimos a .NET que busque el token en la cookie, no en el header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["jwt_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<GameDtoValidator>();
// 6. Configurar OpenAPI para la documentación
builder.Services.AddOpenApi();

var app = builder.Build();

// 7. Pipeline HTTP (Manejo de peticiones)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // OBLIGATORIO: Usar Scalar en lugar del estándar de Swagger
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("API Gestor de Videojuegos");
    });
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // Habilitar las rutas de los controladores

app.Run();