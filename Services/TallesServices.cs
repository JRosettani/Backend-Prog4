using System.Net;
using Backend_Prog4.Config;
using Backend_Prog4.DTOs.Talle;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;

namespace Backend_Prog4.Services;

public class TallesService
{
    private readonly IRepository _repo;
    public TallesService(IRepository db)
    {
        _repo = db;
    }

    public List<Talle> ObtenerTodos()
    {
        var lista = _repo.ObtenerTodos();
        return lista;
    }

    public Talle ObtenerUnoPorId(int id)
    {
        var t = _repo.ObtenerUno((p) => p.Id == id);
        if (t == null)
        {
            throw new RespuestaError(
                $"El talle con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return t;
    }

    public Talle CrearUno(CrearActualizarTalleDTO dto)
    {
        var t = new Talle()
        {
            Nombre = dto.Nombre,
        };

        _repo.CrearUno(t);
        _repo.Guardar();
        return t;
    }

    public Talle ActualizarUnoPorId(int id, CrearActualizarTalleDTO dto)
    {
        Talle talle = ObtenerUnoPorId(id);
        if (dto.Nombre != null)
        {
            talle.Nombre = dto.Nombre;
        }

        _repo.ActualizarUno(talle);
        _repo.Guardar();

        return talle;
    }

    public void EliminarUnoPorId(int id)
    {
        Talle t = ObtenerUnoPorId(id);
        _repo.EliminarUno(t);
        _repo.Guardar();
    }

    /*public List<Talle> ObtenerPorIds(List<int> Ids)
    {
        if (Ids == null || Ids.Count == 0)
        {
            throw new RespuestaError(
               "La lista esta vacia",
               HttpStatusCode.BadRequest
           );
        }
        var talles = _db.Talles.Where(t => Ids.Contains(t.Id)).ToList();
        return talles;
    }*/
}