using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;
using System.Net;

namespace Backend_Prog4.Services
{
    public class EstadoPedidoService
    {
        private readonly IRepository<EstadoPedido> _repo;

        public EstadoPedidoService(IRepository<EstadoPedido> repo)
        {
            _repo = repo;
        }

        public async Task<List<EstadoPedido>> ObtenerTodos()
        {
            return await _repo.ObtenerTodos();
        }
    }
}