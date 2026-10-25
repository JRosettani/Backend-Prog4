namespace Backend_Prog4.Utils.Repuestas
{
    public class Respuesta
    {
        public string Mensaje { get; set; } = string.Empty;
        public object? Datos { get; set; }

        public Respuesta(string msg)
        {
            Mensaje = msg;
        }
        public Respuesta(string msg, object datos)
        {
            Mensaje = msg;
            Datos = datos;
        }

    }
}
