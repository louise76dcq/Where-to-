using dest.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration de Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5000); // Écoute sur toutes les interfaces au port 5000
});

// Ajout des services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SqlConnection>();
builder.Services.AddTransient<DestinationService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policyBuilder => policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Authentification et autorisation
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configuration de l'application
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();