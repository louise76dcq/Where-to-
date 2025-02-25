using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using dest.Services;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services de l'API
builder.Services.AddControllers();
builder.Services.AddSingleton<SqlConnection>();
builder.Services.AddSingleton<DestinationService>();

// Ajouter Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WhereTo API", Version = "v1" });
});

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

// Activer Swagger en mode développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WhereTo API V1");
        c.RoutePrefix = string.Empty; // Swagger sera accessible sur `/`
    });
}

app.Run();