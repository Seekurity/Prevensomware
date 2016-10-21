using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prevensomware.DA
{
    interface IRepository<T>
    {
        void CreateOrUpdate(T obj);
        T Get(Guid oid);
        IEnumerable<T> GetList(Guid oid);
    }
}
