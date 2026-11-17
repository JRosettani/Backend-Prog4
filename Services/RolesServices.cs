using Backend_Prog4.Models;
using Backend_Prog4.Utils.Repuestas;
using Backend_Prog4.Repositories;
using System.Net;

namespace Backend_Prog4.Services;

public class RolesService
{
    private readonly IRepository<Rol> _repo;
    public RolesService(IRepository<Rol> repo)
    {
        _repo = repo;
    }

    public async Task<List<Rol>> ObtenerTodos() => await _repo.ObtenerTodos();

    public async Task<Rol> ObtenerUnoPorNombre(string nombre)
    {
        var rol = await _repo.ObtenerUno((r) => r.Nombre == nombre);
        if (rol == null)
        {
            throw new RespuestaError(
                "El Rol no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return rol;
    }
}