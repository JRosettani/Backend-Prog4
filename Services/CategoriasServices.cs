
using System.Net;
using Backend_Prog4.Config;
using Backend_Prog4.DTOs.Categoria;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;

namespace Backend_Prog4.Services;

public class CategoriasService
{
    private readonly IRepository<Categoria> _repo;

    public CategoriasService(IRepository<Categoria> repo)
    {
        _repo = repo;
    }

    public async Task<List<Categoria>> ObtenerTodos()
    {
        var lista = await _repo.ObtenerTodos();
        return lista;
    }

    public async Task<Categoria> ObtenerUnoPorId(int id)
    {
        var cat = await _repo.ObtenerUno(p => p.Id == id);

        if (cat == null)
        {
            throw new RespuestaError(
                $"La categoria con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }

        return cat;
    }

    public async  Task<Categoria> CrearUno(CrearActualizarCategoriaDTO dto)
    {
        var cat = new Categoria()
        {
            Nombre = dto.Nombre,
        };

        await _repo.CrearUno(cat);

        return cat;
    }

    public async Task<Categoria> ActualizarUnoPorId(int id, CrearActualizarCategoriaDTO dto)
    {
        Categoria cat = await ObtenerUnoPorId(id);

        if (dto.Nombre != null)
        {
            cat.Nombre = dto.Nombre;
        }

        await _repo.ActualizarUno(cat);

        return cat;
    }

    public async Task EliminarUnoPorId(int id)
    {
        Categoria cat = await ObtenerUnoPorId(id);

        await _repo.EliminarUno(cat);
    }

    public async Task<List<Categoria>> ObtenerPorIds(List<int> Ids)
    {
        if (Ids == null || Ids.Count == 0)
        {
            throw new RespuestaError(
                "La lista esta vacia",
                HttpStatusCode.BadRequest
            );
        }

        var cats = await _repo.ObtenerTodos(cat => Ids.Contains(cat.Id));

        return cats;
    }
}
