var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔥 Ajoute ceci pour éviter l'erreur
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication(); // Ajoute l'authentification
app.UseAuthorization();  // Ajoute l'autorisation

app.MapControllers();

app.Run();