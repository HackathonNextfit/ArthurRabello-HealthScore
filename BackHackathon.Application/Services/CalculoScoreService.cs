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

        public CalculoScoreService(IRecuperarPesquisaService iRecuperarPesquisaService, IAvaliacaoFisicaService iAvaliacaoFisicaService, IVendasService _ivendaService)
        {
            _IRecuperarPesquisaService = iRecuperarPesquisaService;
            _IAvaliacaoFisicaService = iAvaliacaoFisicaService;
            _IVendaService = _ivendaService;
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
    }
}