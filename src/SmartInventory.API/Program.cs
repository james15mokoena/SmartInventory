using Microsoft.EntityFrameworkCore;
using SmartInventory.API.Controllers;
using SmartInventory.API.Data;
using SmartInventory.API.Repositories;
using SmartInventory.API.Services;

// load environment variables from .env file.
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// get the connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// get the database password
var password = Environment.GetEnvironmentVariable("SMART_INVENTORY_PASSWORD");
if (string.IsNullOrEmpty(password))
    throw new InvalidOperationException("SMART_INVENTORY_PASSWORD environment variable is required.");

connectionString = connectionString!.Replace("${SMART_INVENTORY_PASSWORD}", password);

// set up the database connection
builder.Services.AddDbContext<DatabaseContext>(options => options.UseMySql(connectionString,new MySqlServerVersion(new Version(8,0))));

builder.Services.AddControllers();

// add repositories to the DI container.
builder.Services.AddScoped<UserManagementRepository>();
builder.Services.AddScoped<SupplierManagementRepository>();
builder.Services.AddScoped<ProductManagementRepository>();

// add services to the DI container
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<UserManagementService>();
builder.Services.AddScoped<SupplierManagementService>();
builder.Services.AddScoped<ProductManagementService>();

// add controllers to the DI container.
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<SupplierController>();
builder.Services.AddScoped<ProductController>();

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