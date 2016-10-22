using System;
using System.Collections.Generic;

namespace Prevensomware.DA
{
    interface IRepository<T>
    { 
        void CreateOrUpdate(T obj);
        void Remove(T obj);
        T Get(int oid);
        IEnumerable<T> GetList();
    }
}
