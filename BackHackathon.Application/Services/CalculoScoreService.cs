using BackHackathon.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackHackathon.Application.Services
{
    public class CalculoScoreService : ICalculoScoreService
    {
        private readonly IRecuperarPesquisaService _IRecuperarPesquisaService;
        private readonly IAvaliacaoFisicaService _IAvaliacaoFisicaService;
        private readonly IVendasService _IVendaService;
        private readonly IAgendamentoAulaService _IAgendamentoAulaService;

        public CalculoScoreService(IRecuperarPesquisaService iRecuperarPesquisaService, IAvaliacaoFisicaService iAvaliacaoFisicaService, IVendasService _ivendaService, IAgendamentoAulaService _iagendamentoAulaService)
        {
            _IRecuperarPesquisaService = iRecuperarPesquisaService;
            _IAvaliacaoFisicaService = iAvaliacaoFisicaService;
            _IVendaService = _ivendaService;
            _IAgendamentoAulaService = _iagendamentoAulaService;
        }

        public async Task<List<Cliente?>> CalcularScore(List<Cliente?> listaCliente)
        {
            foreach (var cliente in listaCliente)
            {
                cliente.Score = cliente.Score + await CalcularScorePresenca(cliente);
                cliente.Score = cliente.Score + await CalcularScoreSemPresenca(cliente);
                cliente.Score = cliente.Score + await CalculaScoreAvaliacaoFisica(cliente);
                cliente.Score = cliente.Score + await CalculaScoreVenda(cliente);
                cliente.Score = cliente.Score + await CalculaScoreTreino(cliente);
                cliente.Score = cliente.Score + await CalculaScoreContasVencidas(cliente);
                cliente.Score = cliente.Score + await CalcularScoreAgendamentoAula(cliente);
                cliente.Score = cliente.Score + await CalculaScoreAgendamentoDesistente(cliente);
                cliente.Score = cliente.Score + await CalculaScoreContratoBloqueado(cliente);
            }
            return listaCliente;
        }
        //Coloquei como horário de pico entre 17 e 20 horas
        public async Task<int> CalcularScorePresenca(Cliente cliente)
        {
            var presencas = await _IRecuperarPesquisaService.RecuperarPessoaPresenca(cliente.Id);
            var quantidadePresencas = presencas.Count;
            var pontoAdicionar = quantidadePresencas * 10;
            foreach (var presencaForadoHoradePico in presencas)
            {
                if (presencaForadoHoradePico.DataHora.Hour < 5 && presencaForadoHoradePico.DataHora.Hour > 8)
                {
                    pontoAdicionar += 5; 
                }
            }
            return pontoAdicionar;
      }
        public async Task<int> CalcularScoreSemPresenca(Cliente cliente)
        {
            var presencas = await _IRecuperarPesquisaService.RecuperarPessoaPresenca(cliente.Id);
            var quantidadePresencas = presencas.Count;
            if (quantidadePresencas == 0)
            {
                return -20;
            }
            return 0;
        }
        public async Task<int> CalculaScoreAvaliacaoFisica(Cliente cliente)
        {
            var avaliacaofisica = await _IAvaliacaoFisicaService.RecuperaAvaliacaoFisica(cliente.Id);

            if (avaliacaofisica == null || avaliacaofisica.Count == 0)
                return 0;

            if (avaliacaofisica.Any(av => av.DataValidade < DateTime.Now))
                return -100;

            return 50;
        }
        public async Task<int> CalculaScoreVenda(Cliente cliente)
        {
            var vendas = await _IVendaService.RecuperarVendas(cliente.Id);

            if (vendas == null || !vendas.Any())
                return 0;

            int score = 0;

            foreach (var venda in vendas)
            {
                if (venda.VendaContrato != null && venda.VendaContrato.Any())
                    score += 100;
                else
                    score += 50;
            }

            return score;
        }

        public async Task<int> CalculaScoreTreino(Cliente cliente)
        {
            var treinos = await _IRecuperarPesquisaService.RecuperaTreino(cliente.Id);
            foreach (var treino in treinos)
            {
                if (treino != null && treino.Status == 2){
                    return 15;
                }
            }
            return 0;
        }
        public async Task<int> CalculaScoreContasVencidas(Cliente cliente)
        {
            var contas = await _IRecuperarPesquisaService.RecuperarContasAbertas(cliente.Id);
            decimal valorcontas = 0;
            int score = 0;
            if (contas == null || !contas.Any())
                return 0;
           
            foreach (var conta in contas)
            {
                if (conta.DataVencimento < DateTime.Now)
                    valorcontas += conta.Valor;              
            }
            int scoreDesconto = (int)(valorcontas / 100) * 10;
            score -= scoreDesconto;

            return score;

        }
        public async Task<int> CalcularScoreAgendamentoAula(Cliente cliente)
        {
            var agendamentos = await _IAgendamentoAulaService.RecuperaAgendamentos(cliente.Id);
            int score = 0;
            foreach (var agendamento in agendamentos)
            {
                if (agendamento is null)
                {
                    continue;
                }

                if (agendamento.Participantes.Any(s => s.Status == 3))
                {
                    score -= 15;
                    continue;
                }

                score += 5;
            }
            return score;
        }
        public async Task<int> CalculaScoreAgendamentoDesistente(Cliente cliente)
        {
            var agendamentos = await _IAgendamentoAulaService.RecuperaAgendamentoDesistente(cliente.Id);
            var quantidadeagendamentos= agendamentos.Count;
            if (quantidadeagendamentos >= 2)
            {
                return -10;
            }
            return 0;
        }
        public async Task<int> CalculaScoreContratoBloqueado(Cliente cliente)
        {
            var contratosbloqueados = await _IVendaService.RecuperaContratosClientes(cliente.Id);
            if (contratosbloqueados.Any())
            {
                return -200;
            }
            return 0;

        }
    }
}
//"Status": 4 suspenso
//"Status": 5, bloqueado