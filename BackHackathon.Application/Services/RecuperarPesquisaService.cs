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
    public async Task<List<Cliente>> Faixa(int alunoId)
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

   public async Task<List<Treino>> RecuperaTreino(int alunoId)
    {
        var client = new HttpClient();
        var filter = $@"[
                                            {{""property"":""CodigoCliente"",""operator"":""equal"",""value"":{alunoId},""and"":true}},
                                            {{""property"":""Inativo"",""operator"":""equal"",""value"":false,""and"":true}}
                                        ]";

                                        var fields = Uri.EscapeDataString(@"[
                                            ""Nome"",""SessaoAtual"",""CodigoCliente"",""DataCriacao"",""Usuario.Nome"",""Observacao"",
                                            ""Id"",""QtdeSessoes"",""ControlaQtdeTreino"",""QtdeTotal"",""QtdeUtilizado"",
                                            ""Status"",""TipoControle"",""DataVencto"",""DataInicio"",""Recomendado"",""Inativo""
                                        ]");

        var url = $"https://api-sandbox.appnext.fit/api/treino?filter={filter}&fields={fields}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);

        var authAppService = new AuthAppService();
        var token = await authAppService.RecuperarToken();

        request.Headers.Add("Authorization", token);

        var response = await client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ResponseApi<List<Treino>>>(responseContent);
        var dataInicio = DateTime.Now.AddDays(-7).Date;
        var treinos = responseDto.Content.Where(treinos => treinos.DataCriacao > dataInicio).ToList();

        return treinos;
    }
    public async Task<List<Contas>> RecuperarContasAbertas(int codigoCliente)
    {
        var client = new HttpClient();
        var filter = "[{\"property\":\"Status\",\"operator\":\"in\",\"value\":[1,2,5],\"and\":true},{\"property\":\"DataVencimento\",\"operator\":\"greaterOrEqual\",\"value\":\"2025-07-01T22:21:04.627Z\",\"and\":true},{\"property\":\"DataVencimento\",\"operator\":\"lessOrEqual\",\"value\":\"2025-07-31T22:21:59.999Z\",\"and\":true}]";
        var includes = "[\"Cliente\",\"TipoReceber\",\"ReceberRecebimento.MetodoPagamento\",\"ReceberRecebimento.MetodoPagamentoConfig\",\"Usuario\",\"ReceberRecebimento\"]";
        var url = $"https://api-sandbox.appnext.fit/api/receber?filter={filter}&includes={includes}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        var authAppService = new AuthAppService();
        var token = await authAppService.RecuperarToken();

        request.Headers.Add("Authorization", token);

        var response = await client.SendAsync(request);

        var responseContent = await response.Content.ReadAsStringAsync();
        var responseDto = JsonSerializer.Deserialize<ResponseApi<List<Contas>>>(responseContent);
        var dataInicio = DateTime.Now.AddDays(-7).Date;
        var contas = responseDto.Content.Where(contas => contas.DataCriacao > dataInicio && contas.CodigoCliente == codigoCliente).ToList();

        return contas;
    }
}





