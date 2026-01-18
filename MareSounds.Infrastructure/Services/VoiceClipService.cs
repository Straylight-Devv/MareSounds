using MareSounds.Core.Interfaces.Repos;
using MareSounds.Core.Interfaces.Services;
using MareSounds.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MareSounds.Infrastructure.Services
{
    public class VoiceClipService : IDBService<VoiceClip>
    {
        private IDBRepo<VoiceClip> _dbRepo;

        public VoiceClipService(IDBRepo<VoiceClip> dBRepo) 
        {
            _dbRepo = dBRepo;
        }

        public List<VoiceClip> Get(List<VoiceClip>? filter = null) => _dbRepo.Get(filter);

        public void Upsert(List<VoiceClip> mares) => _dbRepo.Upsert(mares);

        public void Delete(List<VoiceClip> mares) => _dbRepo.Delete(mares);
    }
}
