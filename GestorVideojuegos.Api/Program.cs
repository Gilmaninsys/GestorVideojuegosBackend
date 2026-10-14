using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using GestorVideojuegos.Api.Middlewares;
using GestorVideojuegos.Application.Interfaces;
using GestorVideojuegos.Application.Services;
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

// 4. Inyección de Dependencias (Clean Architecture)
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ITierListRepository, TierListRepository>();
builder.Services.AddScoped<ITierListService, TierListService>();
builder.Services.AddScoped<IGameService, GestorVideojuegos.Application.Services.GameService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, GestorVideojuegos.Application.Services.AuthService>();

// 5. Agregar soporte para Controladores y AntiForgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN"; // El header que enviará Axios

    // CAMBIO VITAL: Le damos un nombre distinto a la cookie interna de .NET
    options.Cookie.Name = ".AspNetCore.Csrf";

    options.Cookie.HttpOnly = true; // Esta sí debe estar oculta
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// Agregamos el filtro global para exigir el token en POST, PUT y DELETE
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Necesario para el envío de cookies
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

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<CspMiddleware>();

// 7. Pipeline HTTP (Manejo de peticiones)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("API Gestor de Videojuegos");
    });
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");

app.UseAuthentication(); // Va ANTES del Authorization
app.UseMiddleware<ApiKeyMiddleware>(); // Dependiendo de lo que haga este middleware, suele ir aquí
app.UseAuthorization();

app.MapControllers();

app.Run();