using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<T>> ObtenerTodos(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<T?> ObtenerUno(
            Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
                query = query.Where(predicate);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> CrearUno(T entity)
        {
            await _db.AddAsync(entity);
            await Guardar();
            return entity;
        }

        public async Task<T> ActualizarUno(T entity)
        {
            _db.Update(entity);
            await Guardar();
            return entity;
        }

        public async Task EliminarUno(T entity)
        {
            _db.Remove(entity);
            await Guardar();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}