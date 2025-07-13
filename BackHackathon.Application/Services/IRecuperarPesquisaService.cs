
using BackHackathon.Application.Entities;

namespace BackHackathon.Application.Services
{
    public interface IRecuperarPesquisaService
    {
        public Task<List<Cliente?>> RecuperarClientesAtivos();
        public Task<List<PessoaPresenca?>> RecuperarPessoaPresenca(int codigoPessoa);
        public Task<List<Cliente>> MostraScore(int PessoaId);
        public Task<List<Cliente>> Faixa(int alunoId);

    }
}
