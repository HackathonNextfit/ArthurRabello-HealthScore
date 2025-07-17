using BackHackathon.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackHackathon.Application.Services
{
    public interface IVendasService
    {
        public Task<List<Vendas>> RecuperarVendas(int codigoCliente);
<<<<<<< HEAD
=======
        public Task<List<Contratos?>> RecuperaContratosClientes(int codigoCliente);
>>>>>>> c04f7dd
    }
}
