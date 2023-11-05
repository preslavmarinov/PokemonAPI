using Microsoft.AspNetCore.Identity;
using PokemonApi.Common.Configurations;
using PokemonApi.Data;
using PokemonApi.Data.Models.Identity;
using PokemonApi.Services.Interfaces;
using PokemonApi.Services.Seeding;

var builder = WebApplication.CreateBuilder(args);
//const string csvPath = "C:\\Users\\Preslav Marinov\\OneDrive\\Desktop\\pokemon-csv-files\\new-pokemon.csv";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
