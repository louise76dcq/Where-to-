using dest.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<SqlConnection>();
builder.Services.AddTransient<DestinationService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});



// 🔥 Ajoute ceci pour éviter l'erreur
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(builder =>
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthentication(); // Ajoute l'authentification
app.UseAuthorization();  // Ajoute l'autorisation

app.MapControllers();

app.Run();