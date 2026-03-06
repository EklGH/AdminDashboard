using AdminDashboard.Api.GraphQL;
using AdminDashboard.Application.Interfaces;
using AdminDashboard.Application.Services;
using AdminDashboard.Infrastructure.Auth;
using AdminDashboard.Infrastructure.Persistence;
using AdminDashboard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);



// ======== DB

// Paramètres DB (variables .env)
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrEmpty(connectionString))
{
    var host = builder.Configuration["POSTGRES_HOST"] ?? "db";
    var port = builder.Configuration["POSTGRES_PORT"] ?? "5432";
    var db = builder.Configuration["POSTGRES_DB"] ?? "admindashboard";
    var user = builder.Configuration["POSTGRES_USER"] ?? "postgres";
    var password = builder.Configuration["POSTGRES_PASSWORD"] ?? "postgres";
    
    // Chaîne de connexion PostgreSQL
    connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";
}

// DI DbContext
builder.Services.AddDbContextFactory<AppDbContext>(opt =>
    opt.UseNpgsql(connectionString));



// ======== JWT / AUTHENTICATION

// Detecte si on est en design-time EF Core
var isDesignTime = Environment.GetCommandLineArgs().Any(arg => arg.Contains("ef"));

// Paramètres JWT (variables .env)
var jwtKey = isDesignTime ? "DummyKeyForEfDesignTime123!" : builder.Configuration["JWT_KEY"] ?? throw new InvalidOperationException("JWT Key non configurée");
var jwtIssuer = builder.Configuration["JWT_ISSUER"] ?? "AdminDashboardBack";
var jwtAudience = builder.Configuration["JWT_AUDIENCE"] ?? "AdminDashboardFront";

// Configuration JWT runtime uniquement (ignore EF Core)
if (!isDesignTime)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

    // DI Auth
    builder.Services.AddAuthorization();
}




// ======== SERVICES / CONTROLLERS

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Repositories :
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// Auth :
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// Services :
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReservationService, ReservationService>();



// ======== GRAPHQL

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<ProductQuery>()
    .AddType<ProductType>()
    .AddType<PaginatedProductType>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true)
    .AddFiltering()
    .AddSorting();




// ======== DOCKER PORT / Kestrel

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});



// ======== BUILD APPLICATION

var app = builder.Build();



// ======== MIDDLEWARE

if (!isDesignTime)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapGraphQL("/graphql");
}



// ======== DB MIGRATIONS + SEED (dev uniquement)

if (app.Environment.IsDevelopment() && !isDesignTime)
{
    using var scope = app.Services.CreateScope();
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    var db = await dbFactory.CreateDbContextAsync();

    // Retry (*10) pour la DB
    var retries = 10;
    var delay = TimeSpan.FromSeconds(5);

    for (int i = 0; i < retries; i++)
    {
        try
        {
            // Test de connexion
            if (db.Database.CanConnect())
            {
                Console.WriteLine("DB prête !");
                break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DB non prête, retry {i + 1}/{retries} en {delay.Seconds}s...");
            Console.WriteLine($"Erreur : {ex.Message}");
        }

        // si échec après tous les retries
        if (i == retries - 1)
        {
            Console.WriteLine("Échec de la connexion DB après plusieurs tentatives !");
            throw new InvalidOperationException("DB non disponible au démarrage");
        }

        await Task.Delay(delay);
    }

    // Seed (Admin, role par défaut)
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    await DbInitializer.InitializeAsync(db, userService);
}



app.Run();