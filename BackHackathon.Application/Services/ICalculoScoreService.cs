using BackHackathon.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackHackathon.Application.Services
{
    public interface ICalculoScoreService
    {
        public Task<List<Cliente?>> CalcularScore(List<Cliente?> listaCliente);
    }
}
