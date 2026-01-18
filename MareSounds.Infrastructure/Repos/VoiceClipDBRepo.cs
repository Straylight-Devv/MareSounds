using MareSounds.Core.Interfaces.Repos;
using MareSounds.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MareSounds.Infrastructure.Repos
{
    public class VoiceClipDBRepo : IDBRepo<VoiceClip>
    {
        private DBContext _dbContext;

        public VoiceClipDBRepo(DBContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        public List<VoiceClip> Get(List<VoiceClip>? filter)
        {
            lock (_dbContext)
            {
                if (filter == null)
                {
                    return _dbContext.VoiceClips.ToList();
                }

                return _dbContext.VoiceClips.ToList();
            }
        }

        public void Upsert(List<VoiceClip> objs)
        {
            lock (_dbContext)
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.VoiceClips.AddRange(objs);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(List<VoiceClip> objs)
        {
            lock (_dbContext)
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.VoiceClips.RemoveRange(objs);
                _dbContext.SaveChanges();
            }
        }
    }
}
