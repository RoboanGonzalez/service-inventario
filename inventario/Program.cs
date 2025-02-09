using inventario.Context;
using inventario.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configura CORS antes de construir la aplicación
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // Permite solicitudes desde tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var corsPolicy = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar el servicio de PiezaService
builder.Services.AddScoped<PiezaService>();

// Registrar los controladores
builder.Services.AddControllers();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar la tubería de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Habilitar CORS
app.UseCors("AllowLocalhost");

// Mapear los controladores
app.MapControllers();

app.Run();
