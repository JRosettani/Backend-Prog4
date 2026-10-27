using Backend_Prog4.Config;

namespace Backend_Prog4.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> _dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public List<T> ObtenerTodos(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.ToList();
        }

        public T? ObtenerUno(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefault();
        }

        public T CrearUno(T entity)
        {
            _db.Add(entity);
            Guardar();
            return entity;
        }

        public T ActualizarUno(T entity)
        {
            _db.Update(entity);
            Guardar();
            return entity;
        }

        public void EliminarUno(T entity)
        {
            _db.Remove(entity);
            Guardar();
        }

        public void Guardar()
        {
            _db.SaveChanges();
        }
    }
}
