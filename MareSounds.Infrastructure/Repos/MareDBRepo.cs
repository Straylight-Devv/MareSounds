using MareSounds.Core.Interfaces.Repos;
using MareSounds.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MareSounds.Infrastructure.Repos
{
    public class MareDBRepo : IDBRepo<Mare>
    {
        DBContext _dbContext;

        public MareDBRepo(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Mare> Get(List<Mare>? filter)
        {
            lock (_dbContext)
            {
                return _dbContext.Mares.Include(_ => _.VoiceClips).ToList();
            }
        }

        public void Upsert(List<Mare> objs)
        {
            lock (_dbContext)
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.Mares.AddRange(objs.Where(_ => _.Id is null));
                _dbContext.Mares.UpdateRange(objs.Where(_ => _.Id is object));
                _dbContext.SaveChanges();
            }
        }

        public void Delete(List<Mare> objs)
        {
            lock (_dbContext)
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.Mares.RemoveRange(objs);
                _dbContext.SaveChanges();
            }
        }
    }
}
