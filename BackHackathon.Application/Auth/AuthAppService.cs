using System.Text.Json;
using BackHackathon.Application.Auth.Dtos;
using BackHackathon.Application.Exemplo.Dtos;

namespace BackHackathon.Application.Auth;

public class AuthAppService : IAuthAppService
{
    public async Task<LoginResponseDto?> EfetuarLogin(LoginRequestDto requestDto)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api-sandbox.appnext.fit/api/token");
        
        var collection = new List<KeyValuePair<string, string>>
        {
            new("username", requestDto.Usuario),
            new("password", requestDto.Senha),
            new("codigotenant",requestDto.CodigoTenant),
            new("grant_type", "password")
        };

        request.Content = new FormUrlEncodedContent(collection);
        var response = await client.SendAsync(request);
        Console.WriteLine(response.RequestMessage);
        //response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<LoginResponseDto>(responseContent);

        return responseDto;
    }

    public async Task<string> RecuperarToken()
    {
        var responseDto = await EfetuarLogin(new LoginRequestDto()
        {
            Usuario = "angelo.darosa@nextfit.com.br",
            Senha = "123456r",
            CodigoTenant = "7775"
        });
        var acessToken = "Bearer " + responseDto?.AccessToken;
        return acessToken ?? string.Empty;
    }
}