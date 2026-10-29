using Backend_Prog4.Models;

namespace Backend_Prog4.Repositories
{
    public interface IRepositoryObtenerIds<T> : IRepository<T> where T : class
    {
        List<T> ObtenerPorIds(List<int> Ids);

    }
}
