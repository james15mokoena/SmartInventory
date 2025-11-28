using Microsoft.EntityFrameworkCore;
using SmartInventory.API.Controllers;
using SmartInventory.API.Data;
using SmartInventory.API.Repositories;
using SmartInventory.API.Services;

var builder = WebApplication.CreateBuilder(args);

// get the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
                        .Replace("${DB_PASSWORD}",Environment.GetEnvironmentVariable("DB_PASSWORD"));

// set up the database connection
builder.Services.AddDbContext<DatabaseContext>(options => options.UseMySql(connectionString,new MySqlServerVersion(new Version(8,0))));

builder.Services.AddControllers();

// add services to the DI container
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<UserManagementService>();

// add repositories to the DI container.
builder.Services.AddScoped<UserManagementRepository>();

// add controllers to the DI container.
builder.Services.AddScoped<UserController>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();