using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ToDoList.Interfaces;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Filters;

var builder = WebApplication.CreateBuilder(args);

string connectionString = Environment.GetEnvironmentVariable("MYSQL_URL");

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//Dependency inyection
builder.Services.AddScoped<IPasswordHasher<TodoUser>, PasswordHasher<TodoUser>>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddDbContext<TodoContext>(options => options.UseMySQL(connectionString));



//filters
builder.Services.AddScoped<ManualAuthorizationAttribute>();
//authentication Service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes("Hola mundo soy Mario"))
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}
else
{
    await using var scope = app.Services.CreateAsyncScope();
    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
    logger.LogInformation($"StringConnection {connectionString}");
    using var db = scope.ServiceProvider.GetService<TodoContext>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
