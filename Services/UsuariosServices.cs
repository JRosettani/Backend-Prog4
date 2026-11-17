using System.Net;
using Backend_Prog4.DTOs.Usuario;
using Backend_Prog4.Models;
using Backend_Prog4.Utils.Repuestas;
using Backend_Prog4.Config;
using Backend_Prog4.Dtos.Usuario;
using Backend_Prog4.Repositories;
using Backend_Prog4.Enums;

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

    public async Task<List<UsuarioDTO>> ObtenerTodos()
    {
        var lista = await _repo.ObtenerTodos();

        return lista.Select(u => new UsuarioDTO()
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Email = u.Email,
            Roles = u.Roles
        }).ToList();
    }

    public async Task<Usuario> ObtenerUnoPorId(int id)
    {
        var u = await _repo.ObtenerUno((u) => u.Id == id, includes: [u => u.Roles]);
        if (u == null)
        {
            throw new RespuestaError(
                $"El Usuario con id = {id} no se encontro",
                HttpStatusCode.NotFound
            );
        }
        return u;
    }

    public async Task<Usuario> ObtenerUnoPorEmail(string email)
    {
        var u = await _repo.ObtenerUno(
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


    public async Task<Usuario> CrearUno(CrearUsuarioDTO dto)
    {
        var u = await _repo.ObtenerUno(u => u.Email == dto.Email);

        if (u != null)
        {
            throw new RespuestaError(
                "El correo electronico no esta disponible",
                HttpStatusCode.BadRequest
            );
        }

        var rol = await _rolesService.ObtenerUnoPorNombre(ROL.USER);

        Usuario usuario = new Usuario()
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            PasswordHash = _encoder.Encode(dto.Clave),
            Roles = new List<Rol>() { rol }
        };

        await _repo.CrearUno(usuario);

        return usuario;
    }

    public async Task<Usuario> ActualizarUnoPorId(int id, ActualizarUsuarioDTO dto)
    {
        Usuario usuario = await ObtenerUnoPorId(id);

        if (dto.Nombre != null)
        {
            usuario.Nombre = dto.Nombre;
        }

        if (dto.Apellido != null)
        {
            usuario.Apellido = dto.Apellido;
        }

        if (dto.Email != null)
        {
            usuario.Email = dto.Email;
        }

        if (dto.Clave != null)
        {
            // la clave nunca se guarda tal cual: se hashea antes
            usuario.PasswordHash = _encoder.Encode(dto.Clave);
        }

        await _repo.ActualizarUno(usuario);

        return usuario;
    }

}