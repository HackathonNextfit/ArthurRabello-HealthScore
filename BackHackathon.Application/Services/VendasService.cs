using BackHackathon.Application.Auth;
using BackHackathon.Application.Entities;
using BackHackathon.Application.Exemplo.Dtos;
using BackHackathon.Application.Services.Dtos;
using BackHackathon.Application.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace BackHackathon.Application.Services
{
    public class VendasService : IVendasService
    {
        public async Task<List<Vendas?>> RecuperarVendas(int codigoCliente)
        {
            var client = new HttpClient();
            var filtrodatainicio = DateTime.Now.AddDays(-7).Date;
            var filtrodatafinal = DateTime.Now;
            var filter = $@"[
                            {{ ""property"":""CodigoCliente"",""operator"":""equal"",""value"":{codigoCliente},""and"":""true"" }},
                            {{ ""property"":""Status"",""operator"":""in"",""value"":[1, 3, 4, 5],""and"":""true"" }},
                            {{ ""property"":""Data"",""operator"":""greaterOrEqual"",""value"":""{filtrodatainicio}+Z"",""and"":""true"" }},
                            {{ ""property"":""Data"",""operator"":""lessOrEqual"",""value"":""{filtrodatafinal}+Z"",""and"":""true"" }}
                        ]";
            var includes = @"[""Cliente"",""VendaContrato""]";
            var fields = @"[""Id"",""Cliente.Nome"",""CodigoCliente"",""Status"",""CodigoItem"",""ValorTotal"",""VendaContrato.CodigoContratoBase"",""Data"",""Descricao"",""ValorDesconto"",""ValorDescontoVip"",""VendaContrato.ContratoBase.Tipo"",""Gympass"",""Recorrente"",""MotivoErro"",""Origem""]";
            var encodedIncludes = Uri.EscapeDataString(includes);
            var sort = @"[{""property"":""Data"",""direction"":""desc""}]";
            var finalUrl = $"https://api-sandbox.appnext.fit/api/venda?includes={encodedIncludes}&filter={Uri.EscapeDataString(filter)}&fields={Uri.EscapeDataString(fields)}&sort={Uri.EscapeDataString(sort)}&limit=10&page=1";
            var request = new HttpRequestMessage(HttpMethod.Get, finalUrl);
            var authAppService = new AuthAppService();
            var token = await authAppService.RecuperarToken();
            request.Headers.Add("Authorization", token);
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<ResponseApi<List<Vendas>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var dataInicio = DateTime.Now.AddDays(-7).Date;
            var vendas = responseDto.Content
                .Where(v => v.Data.ToUniversalTime() >= dataInicio).ToList();
                

            return vendas;
        }
    }
}


