using PokedexApp.Data;

namespace PokedexApp.Service
{
    public interface IService<T>
    {
        public List<T> GetDataFromApi();
        public IQueryable<T> GetDataFromDatabase();
    }
}
