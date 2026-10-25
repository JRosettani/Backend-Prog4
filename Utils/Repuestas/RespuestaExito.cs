using System.Net;

namespace Backend_Prog4.Utils.Repuestas
{
    public class RespuestaExito
    {
        public HttpStatusCode CodigoEstado { get; }
        public Respuesta Res { get; set; }

        public RespuestaExito(string mensaje, HttpStatusCode codigo)
        {
            Res = new Respuesta(mensaje);
            CodigoEstado = codigo;
        }

        public RespuestaExito(string mensaje, HttpStatusCode codigo, object datos)
        {
            Res = new Respuesta(mensaje, datos);
            CodigoEstado = codigo;
        }
    }
}
