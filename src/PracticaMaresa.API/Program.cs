using Microsoft.EntityFrameworkCore;
using PracticaMaresa.Application.Interfaces;
using PracticaMaresa.Application.Services;
using PracticaMaresa.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddHttpClient<IExternalValidationService, ExternalValidationService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "PracticaMaresa API", Version = "v1", Description = "API REST para registro de pedidos" });
});

var app = builder.Build();

app.UseMiddleware<PracticaMaresa.API.Middlewares.GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PracticaMaresa API v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz http://localhost:5150
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
