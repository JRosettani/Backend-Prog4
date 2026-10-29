using System.Net;
using Backend_Prog4.DTOs.Usuario;
using Backend_Prog4.Models;
using Backend_Prog4.Utils.Repuestas;
using Backend_Prog4.Config;
using Backend_Prog4.Dtos.Usuario;
using Backend_Prog4.Repositories;

namespace Backend_Prog4.Services;

public class UsuariosService
{
    private readonly IRepository<Usuario> _repo;
    private readonly RolesService _rolesService;
    private readonly IEncoderServices _encoder;
    public UsuariosService(IRepository<Usuario> repo, RolesService rolesService, IEncoderServices encoder)
    {
        _repo = repo;
        _rolesService = rolesService;
        _encoder = encoder;
    }

    public List<UsuarioDTO> ObtenerTodos()
    {
        var lista = _repo.ObtenerTodos().Select(u => new UsuarioDTO()
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Roles = u.Roles
        }).ToList();
        return lista;
    }

    public Usuario ObtenerUnoPorId(int id)
    {
        var u = _repo.ObtenerUno((u) => u.Id == id, includes: [u => u.Roles]);
        if (u == null)
        {
            throw new RespuestaError(
                $"El Usuario con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return u;
    }

    public Usuario ObtenerUnoPorEmail(string email)
    {
        var u = _repo.ObtenerUno(
            p => p.Email == email,
            includes: [u => u.Roles]
        );

        if (u == null)
        {
            throw new RespuestaError(
                "El Usuario no se encontro",
                HttpStatusCode.NotFound
            );
        }

        return u;
    }


    public Usuario CrearUno(CrearUsuarioDTO dto)
    {
        var u = _repo.ObtenerUno(u => u.Email == dto.Email);

        if (u != null)
        {
            throw new RespuestaError(
                "El correo electronico no esta disponible",
                HttpStatusCode.BadRequest
            );
        }

        var rol = _rolesService.ObtenerUnoPorNombre("User");

        Usuario usuario = new Usuario()
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            PasswordHash = _encoder.Encode(dto.Clave),
            Roles = new List<Rol>() { rol }
        };

        _repo.CrearUno(usuario);

        return usuario;
    }

}