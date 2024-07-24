using System.Text.Json;
using BackHackathon.Application.Exemplo.Dtos;

namespace BackHackathon.Application.Exemplo;

public class ExemploAppService : IExemploAppService
{
    public async Task<AmbienteUsuarioResponseDto?> RecuperarAmbienteUsuario(AmbienteUsuarioRequestDto requestDtoDto)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.sandbox.appnext.fit/api/usuario/RecuperarAmbienteUsuario");
        
        request.Headers.Add("Authorization", requestDtoDto.Authorization);
        request.Headers.Add("CodigoUsuario", requestDtoDto.CodigoUsuario.ToString());
        request.Headers.Add("Codigo-Unidade", requestDtoDto.CodigoUnidade.ToString());
        
        var response = await client.SendAsync(request);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<AmbienteUsuarioResponseDto>(responseContent);
        
        return responseDto;
    }
    
    public async Task<EnumsResponseDto?> RecuperarEnums(EnumsRequestDto requestDto)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.sandbox.appnext.fit/api/Enum/RecuperarTodos");
        
        request.Headers.Add("Authorization", requestDto.Authorization);
        request.Headers.Add("Accept", "application/json");
        
        var response = await client.SendAsync(request);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<EnumsResponseDto>(responseContent);
        
        return responseDto;
    }
}