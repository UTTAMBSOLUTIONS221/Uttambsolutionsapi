using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactLocalhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Adjust this to your React app's URL
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowReactLocalhost"); // Apply CORS policy to the entire app

app.MapControllers();

app.Run();
