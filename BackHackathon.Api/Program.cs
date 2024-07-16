using BackHackathon.Application.Auth;
using BackHackathon.Application.Exemplo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthAppService, AuthAppService>();
builder.Services.AddScoped<IExemploAppService, ExemploAuthService>();

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