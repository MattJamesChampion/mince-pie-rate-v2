using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MincePieRateV2.DAL.Repositories
{
    public interface IRepository<T>
    {
        T GetEntity(Func<T, bool> predicate);
        IEnumerable<T> GetEntities(Func<T, bool> predicate=null);
    }
}
