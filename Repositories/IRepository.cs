using System.Linq.Expressions;

namespace Backend_Prog4.Repositories

{
    public interface IRepository<T> where T : class
    {
        List<T> ObtenerTodos(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? includes = null
    );
        T? ObtenerUno(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? includes = null
        );
        T CrearUno(T entity);
        T ActualizarUno(T entity);
        void EliminarUno(T entity);
        void Guardar();
    }
}
