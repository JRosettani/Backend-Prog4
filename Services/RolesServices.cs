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

    public List<Rol> ObtenerTodos() => _repo.ObtenerTodos();

    public Rol ObtenerUnoPorNombre(string nombre)
    {
        var rol = _repo.ObtenerUno((r) => r.Nombre == nombre);
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