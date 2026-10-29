
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

    public List<Marca> ObtenerTodos()
    {
        var lista = _repo.ObtenerTodos();
        return lista;
    }

    public Marca ObtenerUnoPorId(int id)
    {
        var m = _repo.ObtenerUno((m) => m.Id == id);
        if (m == null)
        {
            throw new RespuestaError(
                $"La marca con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return m;
    }

    public Marca CrearUno(CrearActualizarMarcaDTO dto)
    {
        var m = new Marca()
        {
            Nombre = dto.Nombre,
        };

        _repo.CrearUno(m);
        return m;
    }

    public Marca ActualizarUnoPorId(int id, CrearActualizarMarcaDTO dto)
    {
        Marca marca = ObtenerUnoPorId(id);
        if (dto.Nombre != null)
        {
            marca.Nombre = dto.Nombre;
        }

        _repo.ActualizarUno(marca);
        _repo.Guardar();

        return marca;
    }

    public void EliminarUnoPorId(int id)
    {
        Marca m = ObtenerUnoPorId(id);
        _repo.EliminarUno(m);
    }

    public List<Marca> ObtenerPorIds(List<int> Ids)
    {
        if (Ids == null || Ids.Count == 0)
        {
            throw new RespuestaError(
               "La lista esta vacia",
               HttpStatusCode.BadRequest
           );
        }
        var marcas = _repo.ObtenerTodos(p => Ids.Contains(p.Id));
        return marcas;
    }
}
