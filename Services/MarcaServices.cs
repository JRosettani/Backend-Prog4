
using System.Net;
using Backend_Prog4.Config;
using Backend_Prog4.DTOs.Talle;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;

namespace Backend_Prog4.Services;

public class MarcaServices
{
    private readonly IRepository<Marca> _repo;

    public MarcaServices(IRepository<Marca> repo)
    {
        _repo = repo;
    }

    public async Task<List<Marca>> ObtenerTodos()
    {
        var lista = await _repo.ObtenerTodos();
        return lista;
    }

    public async  Task<Marca> ObtenerUnoPorId(int id)
    {
        var m =  await _repo.ObtenerUno((m) => m.Id == id);
        if (m == null)
        {
            throw new RespuestaError(
                $"La marca con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return m;
    }

    public async Task<Marca> CrearUno(CrearActualizarMarcaDTO dto)
    {
        var m = new Marca()
        {
            Nombre = dto.Nombre,
        };

        await _repo.CrearUno(m);
        return m;
    }

    public async Task<Marca> ActualizarUnoPorId(int id, CrearActualizarMarcaDTO dto)
    {
        Marca marca = await ObtenerUnoPorId(id);
        if (dto.Nombre != null)
        {
            marca.Nombre = dto.Nombre;
        }

        await _repo.ActualizarUno(marca);
        await _repo.Guardar();

        return marca;
    }

    public async Task EliminarUnoPorId(int id)
    {
        Marca m = await ObtenerUnoPorId(id);
        await _repo.EliminarUno(m);
    }

    public async Task<List<Marca>> ObtenerPorIds(List<int> Ids)
    {
        if (Ids == null || Ids.Count == 0)
        {
            throw new RespuestaError(
               "La lista esta vacia",
               HttpStatusCode.BadRequest
           );
        }
        var marcas = await _repo.ObtenerTodos(p => Ids.Contains(p.Id));
        return marcas;
    }
}
