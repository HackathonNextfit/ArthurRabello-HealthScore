using BackHackathon.Application.Auth;
using BackHackathon.Application.Entities;
using BackHackathon.Application.Exemplo.Dtos;
using BackHackathon.Application.Services.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace BackHackathon.Application.Services;

public class RecuperarPesquisaService : IRecuperarPesquisaService
{
    public async Task<List<Cliente>> RecuperarClientesAtivos()
    {
        var client = new HttpClient();
        var collection = new List<KeyValuePair<string, string>>
        {
            new("VerRemovidos", "false"),
            new("StatusStr", "[1,2,4]"),

        };

        var finalUrl = QueryHelpers.AddQueryString("https://api-sandbox.appnext.fit/api/Cliente/recuperarPesquisaGeral", collection!);

        var request = new HttpRequestMessage(HttpMethod.Get, finalUrl);

        var authAppService = new AuthAppService();
        var token = await authAppService.RecuperarToken();

        request.Headers.Add("Authorization", token);

        var response = await client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ResponseApi<List<Cliente>>>(responseContent);
        var alunos = responseDto.Content;

        return responseDto.Content;
    }
    public async Task<List<PessoaPresenca?>> RecuperarPessoaPresenca(int codigoPessoa)
    {
        var client = new HttpClient();
        var filter = $"[{{\"property\":\"CodigoPessoa\",\"operator\":\"equal\",\"value\":{codigoPessoa},\"and\":true}}]";
        var finalUrl = $"https://api-sandbox.appnext.fit/api/PessoaPresenca?filter={filter}";
        var request = new HttpRequestMessage(HttpMethod.Get, finalUrl);

        var authAppService = new AuthAppService();
        var token = await authAppService.RecuperarToken();

        request.Headers.Add("Authorization", token);

        var response = await client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ResponseApi<List<PessoaPresenca>>>(responseContent);
        var dataInicio = DateTime.Now.AddDays(-7).Date;
        var alunos = responseDto.Content.Where(aluno => aluno.DataHora > dataInicio).ToList();

        return alunos;
    }
    public async Task<List<Cliente>> MostraScore(int PessoaId)
    {
        return await RecuperarClientesAtivos().ContinueWith(task =>
        {
            var clientes = task.Result;
            return clientes.Where(cliente => cliente.Id == PessoaId).ToList();
        });
    }
    public async Task<List<Cliente>> FaixaScore(int alunoId)
    {
        var clientes = await RecuperarClientesAtivos();
        var clienteFiltrados = clientes.Where(clientes => clientes.Id == alunoId).ToList();
        foreach (var cliente in clienteFiltrados)
        {
            if (cliente.Score >= 0 && cliente.Score <= 300){
                cliente.Faixa = "🔴 Risco de abandono";
            }
            else if (cliente.Score >= 301 && cliente.Score <= 700)
            {
                cliente.Faixa = "🟡 Engajamento médio";
            }
            else
            {
                cliente.Faixa = "🟢 Engajado";
            }
        }
        return clienteFiltrados;
    }
}





