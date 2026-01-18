using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MareSounds.Core.Interfaces.Repos
{
    public interface IDBRepo<T>
    {
        List<T> Get(List<T>? filter);
        void Upsert(List<T> objs);
        void Delete(List<T> objs);
    }
}
