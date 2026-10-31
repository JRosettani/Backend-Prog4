
using System.Net;
using Backend_Prog4.Config;
using Backend_Prog4.DTOs.Talle;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;

namespace Backend_Prog4.Services;

public class TallesService
{
    private readonly IRepository<Talle> _repo;

    public TallesService(IRepository<Talle> repo)
    {
        _repo = repo;
    }

    public async Task<List<Talle>> ObtenerTodos()
    {
        var lista = await _repo.ObtenerTodos();
        return lista;
    }

    public async Task<Talle> ObtenerUnoPorId(int id)
    {
        var t = await _repo.ObtenerUno((p) => p.Id == id);
        if (t == null)
        {
            throw new RespuestaError(
                $"El talle con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return t;
    }

    public async Task<Talle> CrearUno(CrearActualizarTalleDTO dto)
    {
        var t = new Talle()
        {
            Nombre = dto.Nombre,
        };

        await _repo.CrearUno(t);
        return t;
    }

    public async Task<Talle> ActualizarUnoPorId(int id, CrearActualizarTalleDTO dto)
    {
        Talle talle = await ObtenerUnoPorId(id);
        if (dto.Nombre != null)
        {
            talle.Nombre = dto.Nombre;
        }

        await _repo.ActualizarUno(talle);
        await _repo.Guardar();

        return talle;
    }

    public async Task EliminarUnoPorId(int id)
    {
        Talle t =  await ObtenerUnoPorId(id);
        await _repo.EliminarUno(t);
    }

    public async Task<List<Talle>> ObtenerPorIds(List<int> Ids)
    {
        if (Ids == null || Ids.Count == 0)
        {
            throw new RespuestaError(
               "La lista esta vacia",
               HttpStatusCode.BadRequest
           );
        }
        var talles =  await _repo.ObtenerTodos(p => Ids.Contains(p.Id));
        return talles;
    }
}
