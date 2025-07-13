using BackHackathon.Application.Entities;

namespace BackHackathon.Application.Services
{
    public interface IAvaliacaoFisicaService
    {
        public Task<List<AvaliacaoFisica?>> RecuperaAvaliacaoFisica(int alunoId);
    }
}
