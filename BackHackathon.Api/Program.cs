using BackHackathon.Application.Auth;
using BackHackathon.Application.Exemplo;
using BackHackathon.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthAppService, AuthAppService>();
builder.Services.AddScoped<IRecuperarPesquisaService, RecuperarPesquisaService>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICalculoScoreService, CalculoScoreService>();
builder.Services.AddScoped<IExemploAppService, ExemploAppService>();
builder.Services.AddScoped<IAvaliacaoFisicaService, AvaliacaoFisicaService>();
builder.Services.AddScoped<IVendasService, VendasService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();