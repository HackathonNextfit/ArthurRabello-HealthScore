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

        public CalculoScoreService(IRecuperarPesquisaService iRecuperarPesquisaService, IAvaliacaoFisicaService iAvaliacaoFisicaService)
        {
            _IRecuperarPesquisaService = iRecuperarPesquisaService;
            _IAvaliacaoFisicaService = iAvaliacaoFisicaService;
        }

        public async Task<List<Cliente?>> CalcularScore(List<Cliente?> listaCliente)
        {
            foreach (var cliente in listaCliente)
            {
                cliente.Score = cliente.Score + await CalcularScorePresenca(cliente);
                cliente.Score = cliente.Score + await CalcularScoreSemPresenca(cliente);
                cliente.Score = cliente.Score + await CalculaScoreAvaliacaoFisica(cliente);
            }
            return listaCliente;
        }
        public async Task<int> CalcularScorePresenca(Cliente cliente)
        {
            var presencas = await _IRecuperarPesquisaService.RecuperarPessoaPresenca(cliente.Id);
            var quantidadePresencas = presencas.Count;
            var pontoAdicionar = quantidadePresencas * 10;
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
        
    }
}