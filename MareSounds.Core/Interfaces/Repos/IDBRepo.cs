namespace MareSounds.Core.Interfaces.Repos
{
    public interface IDBRepo<T>
    {
        List<T> Get(List<T>? filter);
        void Upsert(List<T> objs);
        void Delete(List<T> objs);
    }
}
