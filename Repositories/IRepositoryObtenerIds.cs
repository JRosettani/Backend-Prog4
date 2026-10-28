using Backend_Prog4.Models;

namespace Backend_Prog4.Repositories
{
    public interface IRepositoryObtenerIds : IRepository
    {
        List<T> ObtenerPorIds(List<int> Ids);

    }
}
