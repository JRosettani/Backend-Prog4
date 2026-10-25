using System.Net;
using Backend_Prog4.Config;
using Backend_Prog4.DTOs.Categoria;
using Backend_Prog4.Models;
using Backend_Prog4.Utils.Repuestas;

namespace Backend_Prog4.Services;

public class CategoriasService
{
    private readonly AppDbContext _db;
    public CategoriasService(AppDbContext db)
    {
        _db = db;
    }

    public List<Categoria> ObtenerTodos()
    {
        var lista = _db.Categorias.ToList();
        return lista;
    }

    public Categoria ObtenerUnoPorId(int id)
    {
        var cat = _db.Categorias.FirstOrDefault((p) => p.Id == id);
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

        _db.Categorias.Add(cat);
        _db.SaveChanges();
        return cat;
    }

    public Categoria ActualizarUnoPorId(int id, CrearActualizarCategoriaDTO dto)
    {
        Categoria ing = ObtenerUnoPorId(id);
        if (dto.Nombre != null)
        {
            ing.Nombre = dto.Nombre;
        }

        _db.Categorias.Update(ing);
        _db.SaveChanges();

        return ing;
    }

    public void EliminarUnoPorId(int id)
    {
        Categoria cat = ObtenerUnoPorId(id);
        _db.Categorias.Remove(cat);
        _db.SaveChanges();
    }

    public List<Categoria> ObtenerPorIds(List<int> Ids)
    {
        if (Ids == null || Ids.Count == 0)
        {
            throw new RespuestaError(
               "La lista esta vacia",
               HttpStatusCode.BadRequest
           );
        }
        var cats = _db.Categorias.Where(cat => Ids.Contains(cat.Id)).ToList();
        return cats;
    }
}