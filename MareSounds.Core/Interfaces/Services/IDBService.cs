namespace MareSounds.Core.Interfaces.Services
{
    public interface IDBService<T>
    {
        public List<T> Get(List<T>? filter = null);
        public void Upsert(List<T> mares);
        public void Delete(List<T> mares);
    }
}
