using Backend_Prog4.DTOs.Auth;

namespace Backend_Prog4.Services
{
    public interface IAuthService
    {
        public LoginRespuestaDTO Login(LoginDTO login);

    }
}
