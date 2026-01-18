using MareSounds.Core.Interfaces.Repos;
using MareSounds.Core.Interfaces.Services;
using MareSounds.Core.Models;

namespace MareSounds.Infrastructure.Services
{
    public class MareService : IDBService<Mare>
    {
        private IDBRepo<Mare> _dbRepo;

        public MareService(IDBRepo<Mare> dBRepo)
        {
            _dbRepo = dBRepo;
        }

        public List<Mare> Get(List<Mare>? filter = null) => _dbRepo.Get(filter);

        public void Upsert(List<Mare> mares) => _dbRepo.Upsert(mares);

        public void Delete(List<Mare> mares) => _dbRepo.Delete(mares);
    }
}
