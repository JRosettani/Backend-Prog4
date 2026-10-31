using System.Linq.Expressions;

namespace Backend_Prog4.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> ObtenerTodos(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? includes = null
        );

        Task<T?> ObtenerUno(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? includes = null
        );

        Task<T> CrearUno(T entity);

        Task<T> ActualizarUno(T entity);

        Task EliminarUno(T entity);

        Task Guardar();
    }
}