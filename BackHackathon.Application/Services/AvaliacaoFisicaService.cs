using BackHackathon.Application.Auth;
using BackHackathon.Application.Entities;
using BackHackathon.Application.Exemplo.Dtos;
using BackHackathon.Application.Services.Dtos;
using BackHackathon.Application.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace BackHackathon.Application.Services;

public class AvaliacaoFisicaService : IAvaliacaoFisicaService
{
    public async Task<List<AvaliacaoFisica>> RecuperaAvaliacaoFisica(int alunoId)
        {
            var client = new HttpClient();
    
            var finalUrl = "https://api-sandbox.appnext.fit/api/avaliacaoFisica";

            var request = new HttpRequestMessage(HttpMethod.Get, finalUrl);

            var authAppService = new AuthAppService();
            var token = await authAppService.RecuperarToken();

            request.Headers.Add("Authorization", token);

            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<ResponseApi<List<AvaliacaoFisica?>>>(responseContent);
            var alunos = responseDto.Content.Where(alunos => alunos.CodigoCliente == alunoId).ToList();

            return responseDto.Content;
        }
}

