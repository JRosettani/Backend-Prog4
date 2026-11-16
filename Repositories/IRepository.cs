using System.Linq.Expressions;

namespace Backend_Prog4.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> ObtenerTodos(
     Expression<Func<T, bool>>? predicate = null,
     Expression<Func<T, object>>[]? includes = null,
     Func<IQueryable<T>, IQueryable<T>>? thenIncludes = null
 );

        Task<T?> ObtenerUno(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? includes = null,
            Func<IQueryable<T>, IQueryable<T>>? thenIncludes = null
        );

        // Devuelve una pagina de resultados y el total de registros que cumplen el filtro
        Task<(List<T> items, int total)> ObtenerPaginado(
            int pagina,
            int tamanio,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IQueryable<T>>? thenIncludes = null,
            Expression<Func<T, object>>? orderBy = null
        );

        Task<T> CrearUno(T entity);

        Task<T> ActualizarUno(T entity);

        Task EliminarUno(T entity);

        Task Guardar();
    }
}