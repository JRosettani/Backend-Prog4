using System.Net;
using Backend_Prog4.Config;
using Backend_Prog4.DTOs.Categoria;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;

namespace Backend_Prog4.Services;

public class CategoriasService
{
    private readonly IRepository _repo;
    public CategoriasService(IRepository db)
    {
        _repo = db;
    }

    public List<Categoria> ObtenerTodos()
    {
        var lista = _repo.ObtenerTodos();
        return lista;
    }

    public Categoria ObtenerUnoPorId(int id)
    {
        var cat = _repo.ObtenerUno((p) => p.Id == id);
        if (cat == null)
        {
            throw new RespuestaError(
                $"La categoria con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return cat;
    }

    public Categoria CrearUno(CrearActualizarCategoriaDTO dto)
    {
        var cat = new Categoria()
        {
            Nombre = dto.Nombre,
        };

        _repo.CrearUno(cat);
        _repo.Guardar();
        return cat;
    }

    public Categoria ActualizarUnoPorId(int id, CrearActualizarCategoriaDTO dto)
    {
        Categoria cat = ObtenerUnoPorId(id);
        if (dto.Nombre != null)
        {
            cat.Nombre = dto.Nombre;
        }

        _repo.ActualizarUno(cat);
        _repo.Guardar();

        return cat;
    }

    public void EliminarUnoPorId(int id)
    {
        Categoria cat = ObtenerUnoPorId(id);
        _repo.EliminarUno(cat);
        _repo.Guardar();
    }

    /*public List<Categoria> ObtenerPorIds(List<int> Ids)
    {
        if (Ids == null || Ids.Count == 0)
        {
            throw new RespuestaError(
               "La lista esta vacia",
               HttpStatusCode.BadRequest
           );
        }
        var cats = _repo.ObtenerTodos(cat => Ids.Contains(cat.Id)).ToList();
        return cats;
    }*/
}