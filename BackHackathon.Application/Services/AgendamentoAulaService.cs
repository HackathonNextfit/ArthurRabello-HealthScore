using BackHackathon.Application.Auth;
using BackHackathon.Application.Entities;
using BackHackathon.Application.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Text.Json;

namespace BackHackathon.Application.Services
{
    public class AgendamentoAulaService : IAgendamentoAulaService
    {
        private readonly IMemoryCache _cache;

        public AgendamentoAulaService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<List<AgendamentoDeAula>> RecuperaAgendamentos(int alunoId)
        {
            var client = new HttpClient();
            var hoje = DateTime.Now.Date;
            var dataInicial = hoje.AddDays(-(int)hoje.DayOfWeek + (hoje.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));
            var dataFinal = dataInicial.AddDays(6);

            string dataInicialStr = dataInicial.ToString("dd/MM/yyyy");
            string dataFinalStr = dataFinal.ToString("dd/MM/yyyy");

            var url = $"https://api-sandbox.appnext.fit/api/Agenda/ListarView" +
                      $"?DataInicialStr={Uri.EscapeDataString(dataInicialStr)}" +
                      $"&DataFinalStr={Uri.EscapeDataString(dataFinalStr)}" +
                      $"&VerFinalizadas=true" +
                      $"&Tipo=2" +
                      $"&VerParticipantes=true" +
                      $"&page=1&limit=30";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var authAppService = new AuthAppService();
            var token = await authAppService.RecuperarToken();

            request.Headers.Add("Authorization", token);

            var response = await client.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonSerializer.Deserialize<ResponseApi<List<AgendamentoDeAula>>>(responseContent);
            var cacheagendamentos = responseDto.Content;
            var options = new MemoryCacheEntryOptions()
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _cache.Set("Agendamentos-Gerais", cacheagendamentos, options);
            var agendamentos = responseDto.Content
                                                .Where(a =>
                                                    a.Participantes.Any(p => p.CodigoCliente == alunoId) &&
                                                    a.DataInicial >= dataInicial &&
                                                    a.DataFinal <= dataFinal)
                                                .ToList();
            
            return agendamentos;
        }
        public async Task<List<AgendamentoDesistentes?>> RecuperaAgendamentoDesistente(int codigoCliente)
        {
            var recuperaagentementos = await RecuperaAgendamentos(codigoCliente);
            var agendamentos = _cache.Get<List<AgendamentoDeAula>>("Agendamentos-Gerais");
            var cliente = new HttpClient();
            var authAppService = new AuthAppService();
            var token = await authAppService.RecuperarToken();

            cliente.DefaultRequestHeaders.Add("Authorization", token);

            var desistencias = new List<AgendamentoDesistentes?>();

            foreach (var agendamento in agendamentos)
            {
                var url = $"https://api-sandbox.appnext.fit/api/agenda/recuperarDetalhes?Id={agendamento.Id}";
                var response = await cliente.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    continue;

                var content = await response.Content.ReadAsStringAsync();
                var detalhes = JsonSerializer.Deserialize<ResponseApi<AgendamentoDesistentes>>(content);
                desistencias.Add(detalhes.Content);
                //var agendamentodesistentes = detalhes.Content.AgendaParticipantesCancelados.Where(p => p.CodigoCliente == codigoCliente && p.Status == 8).ToList();
                //desistencias.AddRange(agendamentodesistentes.Select(p => new AgendamentoDesistentes
                //{
                //    AgendaParticipantesCancelados = new List<AgendaParticipanteCancelado> { p }
                //}));
            }


            return desistencias;
        }
    }
}
