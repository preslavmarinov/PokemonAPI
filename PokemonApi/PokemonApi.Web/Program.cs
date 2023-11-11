using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using PokemonApi.Common.Configurations;
using PokemonApi.Data;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Services;
using PokemonApi.Services.Interfaces;
using PokemonApi.Services.Seeding;
using PokemonApi.Web.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//const string csvPath = "C:\\Users\\Preslav Marinov\\OneDrive\\Desktop\\pokemon-csv-files\\new-pokemon.csv";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    const string SecurityDefinitionName = "Bearer token";

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = builder.Configuration.GetSection("AppName").Value,
        Version = "v1",
    });

    options.AddSecurityDefinition(SecurityDefinitionName, new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme.Example: \"Bearer {token}\"",
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>(true, SecurityDefinitionName);
});

builder.Services.AddSqlServer<PokemonDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.User.RequireUniqueEmail= true;
        options.Password.RequireNonAlphanumeric= false;
        options.Password.RequireUppercase= false;
        options.Password.RequireLowercase= false;
    })
    .AddEntityFrameworkStores<PokemonDbContext>();

builder.Services.AddTransient<ISeeder, Seeder>();
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ITypeService, TypeService>();


var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSecret").Value);

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = false;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();


var csvPath = builder.Configuration.GetSection("CsvPath").Get<CsvPathConfiguration>();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();

    await seeder.SeedAsync(csvPath.Pokemons);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.SeedRolesAsync();
await app.SeedUsersAsync();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
