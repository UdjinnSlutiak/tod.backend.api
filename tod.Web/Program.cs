using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tod.Domain;

var builder = WebApplication.CreateBuilder(args);

// Adding all necessary services into web project

var services = builder.Services;

services.AddControllers();

services.AddDbContext<ProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// services.AddSwaggerGen();

// Building web application and adding all necessary middlewares

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
