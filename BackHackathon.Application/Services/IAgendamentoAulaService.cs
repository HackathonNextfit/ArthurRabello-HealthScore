using BackHackathon.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackHackathon.Application.Services
{
    public interface IAgendamentoAulaService
    {
        public Task<List<AgendamentoDeAula?>> RecuperaAgendamentos(int alunoId);
        public Task<List<AgendamentoDesistentes?>> RecuperaAgendamentoDesistente(int CodigoCliente);
    }
}
